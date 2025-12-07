namespace Net9Odev.DTOs;

// İŞTE EKSİK OLAN PARÇA BU: Service katmanı bunu arıyor
public record ArtistDto(
    int Id, 
    string Name, 
    string Bio
);

// Yeni ekleme yaparken kullandığımız paket
public record CreateArtistDto(
    string Name,
    string Bio,
    int? LabelId
);

// Güncelleme paketi
public record UpdateArtistDto(
    string Name,
    string Bio,
    int? LabelId
);