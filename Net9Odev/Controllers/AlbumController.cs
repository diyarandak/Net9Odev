using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Net9Odev.Data;
using Net9Odev.DTOs;
using Net9Odev.Entities;

namespace Net9Odev.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlbumController : ControllerBase
{
    private readonly AppDbContext _context;

    public AlbumController(AppDbContext context)
    {
        _context = context;
    }

    // 1. TÜM ALBÜMLERİ GETİR
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var albums = await _context.Albums.ToListAsync();

        var dtos = albums.Select(a => new AlbumResponseDto(
            a.Id,
            a.Name,
            a.Price,
            a.ReleaseDate,
            a.ArtistId,
            a.CreatedAt
        )).ToList();

        return Ok(new
        {
            success = true,
            message = "Albümler listelendi",
            data = dtos
        });
    }

    // 2. YENİ ALBÜM EKLE
    [HttpPost]
    public async Task<IActionResult> Create(CreateAlbumDto request)
    {
        // Önce böyle bir sanatçı var mı diye kontrol edelim (Güvenlik)
        var artistExists = await _context.Artists.AnyAsync(a => a.Id == request.ArtistId);
        if (!artistExists)
        {
            return BadRequest(new { success = false, message = "Böyle bir sanatçı bulunamadı!" });
        }

        var newAlbum = new Album
        {
            Name = request.Name,
            Price = request.Price,
            ReleaseDate = request.ReleaseDate,
            ArtistId = request.ArtistId
        };

        _context.Albums.Add(newAlbum);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            success = true,
            message = "Albüm başarıyla eklendi",
            data = new { newAlbum.Id }
        });
    }
}