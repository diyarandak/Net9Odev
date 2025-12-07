using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Net9Odev.DTOs;
using Net9Odev.Services;

namespace Net9Odev.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LabelController : ControllerBase
{
    private readonly ILabelService _service;
    public LabelController(ILabelService service) { _service = service; }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var data = await _service.GetAllAsync();
        return Ok(ApiResponse<List<LabelResponseDto>>.Ok(data, "Plak şirketleri listelendi"));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result is null 
            ? NotFound(ApiResponse<object>.Fail("Plak şirketi bulunamadı")) 
            : Ok(ApiResponse<LabelResponseDto>.Ok(result));
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(CreateLabelDto request)
    {
        var id = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id }, ApiResponse<object>.Ok(new { id }, "Plak şirketi eklendi"));
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateLabelDto request) => 
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