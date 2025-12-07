using Net9Odev.DTOs;

namespace Net9Odev.Services;

public interface IArtistService
{
    // 1. Listeleme (Read)
    Task<List<ArtistDto>> GetAllArtistsAsync();
    
    // 2. Tek Kayıt Getirme (Read - Detail)
    Task<ArtistDto?> GetArtistByIdAsync(int id);

    // 3. Ekleme (Create)
    Task<int> AddArtistAsync(CreateArtistDto request);
    
    // 4. Güncelleme (Update) - Başarılı mı diye bool döner
    Task<bool> UpdateArtistAsync(int id, UpdateArtistDto request);
    
    // 5. Silme (Delete) - Başarılı mı diye bool döner
    Task<bool> DeleteArtistAsync(int id);
}