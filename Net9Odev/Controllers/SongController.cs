using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Net9Odev.DTOs;
using Net9Odev.Services;

namespace Net9Odev.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SongController : ControllerBase
{
    private readonly ISongService _service;
    public SongController(ISongService service) { _service = service; }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var data = await _service.GetAllAsync();
        return Ok(ApiResponse<List<SongResponseDto>>.Ok(data, "Şarkılar listelendi"));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result is null 
            ? NotFound(ApiResponse<object>.Fail("Şarkı bulunamadı")) 
            : Ok(ApiResponse<SongResponseDto>.Ok(result));
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(CreateSongDto request)
    {
        try 
        { 
            var id = await _service.CreateAsync(request);
            // 201 Created döndürüyoruz
            return CreatedAtAction(nameof(GetById), new { id }, ApiResponse<object>.Ok(new { id }, "Şarkı başarıyla oluşturuldu"));
        }
        catch (Exception ex) { return BadRequest(ApiResponse<object>.Fail(ex.Message)); }
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateSongDto request)
    {
        return await _service.UpdateAsync(id, request) 
            ? Ok(ApiResponse<object>.Ok(null, "Şarkı güncellendi")) 
            : NotFound(ApiResponse<object>.Fail("Şarkı bulunamadı"));
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        return await _service.DeleteAsync(id) 
            ? Ok(ApiResponse<object>.Ok(null, "Şarkı silindi")) 
            : NotFound(ApiResponse<object>.Fail("Şarkı bulunamadı"));
    }
}