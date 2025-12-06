using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Net9Odev.Data;
using Net9Odev.DTOs;
using Net9Odev.Entities;

namespace Net9Odev.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SongController : ControllerBase
{
    private readonly AppDbContext _context;

    public SongController(AppDbContext context)
    {
        _context = context;
    }

    // 1. TÜM ŞARKILARI GETİR
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var songs = await _context.Songs.ToListAsync();

        var dtos = songs.Select(s => new SongResponseDto(
            s.Id,
            s.Name,
            s.DurationSeconds,
            s.TrackNumber,
            s.AlbumId,
            s.CreatedAt
        )).ToList();

        return Ok(new
        {
            success = true,
            message = "Şarkılar listelendi",
            data = dtos
        });
    }

    // 2. YENİ ŞARKI EKLE
    [HttpPost]
    public async Task<IActionResult> Create(CreateSongDto request)
    {
        // Böyle bir albüm var mı kontrolü (Güvenlik)
        var albumExists = await _context.Albums.AnyAsync(a => a.Id == request.AlbumId);
        if (!albumExists)
        {
            return BadRequest(new { success = false, message = "Belirtilen albüm bulunamadı!" });
        }

        var newSong = new Song
        {
            Name = request.Name,
            DurationSeconds = request.DurationSeconds,
            TrackNumber = request.TrackNumber,
            AlbumId = request.AlbumId
        };

        _context.Songs.Add(newSong);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            success = true,
            message = "Şarkı başarıyla eklendi",
            data = new { newSong.Id }
        });
    }
}