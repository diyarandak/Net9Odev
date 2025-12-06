using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Net9Odev.Data;
using Net9Odev.DTOs;
using Net9Odev.Entities;

namespace Net9Odev.Controllers;

[ApiController]
[Route("api/[controller]")] // Bu sayede adres: api/artist olacak
public class ArtistController : ControllerBase
{
    private readonly AppDbContext _context;

    public ArtistController(AppDbContext context)
    {
        _context = context;
    }

    // 1. TÜM SANATÇILARI GETİR (GET)
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var artists = await _context.Artists.ToListAsync();

        // Entity'leri DTO'ya çeviriyoruz (Ödev kuralı)
        var dtos = artists.Select(a => new ArtistResponseDto(
            a.Id, 
            a.Name, 
            a.Bio, 
            a.LabelId, 
            a.CreatedAt
        )).ToList();

        // Ödevin istediği Standart Format
        return Ok(new
        {
            success = true,
            message = "Sanatçılar listelendi",
            data = dtos
        });
    }

    // 2. YENİ SANATÇI EKLE (POST)
    [HttpPost]
    public async Task<IActionResult> Create(CreateArtistDto request)
    {
        var newArtist = new Artist
        {
            Name = request.Name,
            Bio = request.Bio,
            LabelId = request.LabelId
        };

        _context.Artists.Add(newArtist);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            success = true,
            message = "Sanatçı başarıyla eklendi",
            data = new { newArtist.Id }
        });
    }
}