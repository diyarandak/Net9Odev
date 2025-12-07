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
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(CreateLabelDto request) => Ok(new { id = await _service.CreateAsync(request) });

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateLabelDto request) => await _service.UpdateAsync(id, request) ? Ok("Güncellendi") : NotFound();

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) => await _service.DeleteAsync(id) ? Ok("Silindi") : NotFound();
}