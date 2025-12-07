using Microsoft.EntityFrameworkCore;
using Net9Odev.Entities;

namespace Net9Odev.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Artist> Artists { get; set; }
    public DbSet<Album> Albums { get; set; }
    public DbSet<Song> Songs { get; set; }
    public DbSet<Label> Labels { get; set; }
    public DbSet<Concert> Concerts { get; set; }

    // OTOMATİK TARİH AYARI (CreatedAt ve UpdatedAt)
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow; // Güncellenince tarihi bas
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}