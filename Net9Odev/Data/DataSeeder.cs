using Net9Odev.Entities;

namespace Net9Odev.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        // Veritabanı boşsa veri ekle
        if (!context.Users.Any())
        {
            var adminUser = new User
            {
                FullName = "Admin User",
                Email = "admin@music.com",
                Password = "123", // Normalde hashlenmeli
                Role = "Admin",
                CreatedAt = DateTime.UtcNow
            };
            
            var normalUser = new User
            {
                FullName = "Normal User",
                Email = "user@music.com",
                Password = "123",
                Role = "User",
                CreatedAt = DateTime.UtcNow
            };

            context.Users.AddRange(adminUser, normalUser);
            await context.SaveChangesAsync();
        }
    }
}