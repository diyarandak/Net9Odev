using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Net9Odev.DTOs;
using Net9Odev.Services;

namespace Net9Odev.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlbumController : ControllerBase
{
    private readonly IAlbumService _albumService;

    public AlbumController(IAlbumService albumService)
    {
        _albumService = albumService;
    }

    // 1. GET ALL
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _albumService.GetAllAsync();
        return Ok(new { success = true, data = result });
    }

    // 2. GET BY ID (Yeni)
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _albumService.GetByIdAsync(id);
        if (result == null) return NotFound(new { success = false, message = "Albüm bulunamadı" });
        return Ok(new { success = true, data = result });
    }

    // 3. CREATE
    [Authorize] // Kilitli
    [HttpPost]
    public async Task<IActionResult> Create(CreateAlbumDto request)
    {
        try
        {
            var id = await _albumService.CreateAsync(request);
            return Ok(new { success = true, message = "Albüm eklendi", data = new { id } });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    // 4. UPDATE (Yeni)
    [Authorize] // Kilitli
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateAlbumDto request)
    {
        try
        {
            var success = await _albumService.UpdateAsync(id, request);
            if (!success) return NotFound(new { success = false, message = "Albüm bulunamadı" });
            return Ok(new { success = true, message = "Albüm güncellendi" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    // 5. DELETE (Yeni)
    [Authorize] // Kilitli
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _albumService.DeleteAsync(id);
        if (!success) return NotFound(new { success = false, message = "Albüm bulunamadı" });
        return Ok(new { success = true, message = "Albüm silindi" });
    }
}