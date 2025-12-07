namespace Net9Odev.Entities;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Oluşturulma Tarihi
    public DateTime? UpdatedAt { get; set; } // GÜNCELLENME TARİHİ (Bu Eksikti)
}