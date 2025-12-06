namespace Net9Odev.DTOs;

// 1. Kullanıcıya veri gösterirken kullanacağımız paket
public record ArtistResponseDto(
    int Id, 
    string Name, 
    string Bio, 
    int? LabelId, 
    DateTime CreatedAt
);

// 2. Yeni Sanatçı eklerken isteyeceğimiz paket
public record CreateArtistDto(
    string Name, 
    string Bio, 
    int? LabelId
);

// 3. Güncelleme yaparken isteyeceğimiz paket
public record UpdateArtistDto(
    string Name, 
    string Bio, 
    int? LabelId
);