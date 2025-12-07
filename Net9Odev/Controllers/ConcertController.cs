using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Net9Odev.DTOs;
using Net9Odev.Services;

namespace Net9Odev.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConcertController : ControllerBase
{
    private readonly IConcertService _service;
    public ConcertController(IConcertService service) { _service = service; }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var data = await _service.GetAllAsync();
        return Ok(ApiResponse<List<ConcertResponseDto>>.Ok(data, "Konserler listelendi"));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result is null 
            ? NotFound(ApiResponse<object>.Fail("Konser bulunamadı")) 
            : Ok(ApiResponse<ConcertResponseDto>.Ok(result));
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(CreateConcertDto request)
    {
        try 
        { 
            var id = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id }, ApiResponse<object>.Ok(new { id }, "Konser eklendi"));
        }
        catch (Exception ex) { return BadRequest(ApiResponse<object>.Fail(ex.Message)); }
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateConcertDto request) => 
        await _service.UpdateAsync(id, request) 
            ? Ok(ApiResponse<object>.Ok(null, "Güncellendi")) 
            : NotFound(ApiResponse<object>.Fail("Bulunamadı"));

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) => 
        await _service.DeleteAsync(id) 
            ? Ok(ApiResponse<object>.Ok(null, "Silindi")) 
            : NotFound(ApiResponse<object>.Fail("Bulunamadı"));
}