using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Net9Odev.Data;
using Net9Odev.DTOs;
using Net9Odev.Entities;

namespace Net9Odev.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConcertController : ControllerBase
{
    private readonly AppDbContext _context;

    public ConcertController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var concerts = await _context.Concerts.ToListAsync();
        var dtos = concerts.Select(c => new ConcertResponseDto(
            c.Id, c.Venue, c.City, c.Date, c.ArtistId, c.CreatedAt)).ToList();

        return Ok(new { success = true, message = "Konserler listelendi", data = dtos });
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateConcertDto request)
    {
        // Sanatçı kontrolü
        var artistExists = await _context.Artists.AnyAsync(a => a.Id == request.ArtistId);
        if (!artistExists)
            return BadRequest(new { success = false, message = "Sanatçı bulunamadı!" });

        var newConcert = new Concert
        {
            Venue = request.Venue,
            City = request.City,
            Date = request.Date,
            ArtistId = request.ArtistId
        };

        _context.Concerts.Add(newConcert);
        await _context.SaveChangesAsync();

        return Ok(new { success = true, message = "Konser başarıyla eklendi", data = new { newConcert.Id } });
    }
}