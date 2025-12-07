using Microsoft.EntityFrameworkCore;
using Net9Odev.Data;
using Net9Odev.DTOs;
using Net9Odev.Entities;

namespace Net9Odev.Services;

public class ArtistService : IArtistService
{
    private readonly AppDbContext _context;

    public ArtistService(AppDbContext context)
    {
        _context = context;
    }

    // Listeleme
    public async Task<List<ArtistDto>> GetAllArtistsAsync()
    {
        var artists = await _context.Artists.ToListAsync();
        return artists.Select(a => new ArtistDto(a.Id, a.Name, a.Bio)).ToList();
    }

    // Tek Kayıt Getirme
    public async Task<ArtistDto?> GetArtistByIdAsync(int id)
    {
        var artist = await _context.Artists.FindAsync(id);
        if (artist == null) return null;
        return new ArtistDto(artist.Id, artist.Name, artist.Bio);
    }

    // Ekleme
    public async Task<int> AddArtistAsync(CreateArtistDto request)
    {
        var newArtist = new Artist
        {
            Name = request.Name,
            Bio = request.Bio,
            LabelId = request.LabelId
        };

        _context.Artists.Add(newArtist);
        await _context.SaveChangesAsync();
        return newArtist.Id;
    }

    // Güncelleme
    public async Task<bool> UpdateArtistAsync(int id, UpdateArtistDto request)
    {
        var artist = await _context.Artists.FindAsync(id);
        if (artist == null) return false; // Bulunamadı

        // Verileri güncelle
        artist.Name = request.Name;
        artist.Bio = request.Bio;
        if(request.LabelId.HasValue) artist.LabelId = request.LabelId.Value;

        await _context.SaveChangesAsync();
        return true;
    }

    // Silme
    public async Task<bool> DeleteArtistAsync(int id)
    {
        var artist = await _context.Artists.FindAsync(id);
        if (artist == null) return false; // Zaten yok

        _context.Artists.Remove(artist);
        await _context.SaveChangesAsync();
        return true;
    }
}