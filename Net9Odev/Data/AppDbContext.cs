using Microsoft.EntityFrameworkCore;
using Net9Odev.Entities;

namespace Net9Odev.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Tablolarımız buraya tanımlanıyor
    public DbSet<User> Users { get; set; }
    public DbSet<Artist> Artists { get; set; }
    public DbSet<Album> Albums { get; set; }
    public DbSet<Song> Songs { get; set; }
    public DbSet<Label> Labels { get; set; }
    public DbSet<Concert> Concerts { get; set; }
}