using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Net9Odev.Data;
using Net9Odev.DTOs;
using Net9Odev.Entities;

namespace Net9Odev.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _context;

    public UserController(AppDbContext context)
    {
        _context = context;
    }

    // 1. TÜM KULLANICILARI GETİR (GET)
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _context.Users.ToListAsync();
        
        // Şifreleri göndermiyoruz, sadece güvenli bilgileri DTO ile yolluyoruz
        var dtos = users.Select(u => new UserResponseDto(
            u.Id, u.FullName, u.Email, u.Role, u.CreatedAt
        )).ToList();

        return Ok(new { success = true, message = "Kullanıcılar listelendi", data = dtos });
    }

    // 2. KAYIT OL (Register)
    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterDto request)
    {
        // Aynı e-posta var mı kontrolü
        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
        {
            return Conflict(new { success = false, message = "Bu e-posta adresi zaten kayıtlı!" });
        }

        var newUser = new User
        {
            FullName = request.FullName,
            Email = request.Email,
            Password = request.Password, // Not: Gerçek projede şifrelenmeli
            Role = request.Role
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return Ok(new { success = true, message = "Kullanıcı başarıyla oluşturuldu", data = new { newUser.Id } });
    }

    // 3. GİRİŞ YAP (Login)
    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginDto request)
    {
        // Kullanıcıyı e-posta ve şifresine göre ara
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email && u.Password == request.Password);

        if (user == null)
        {
            return Unauthorized(new { success = false, message = "E-posta veya şifre hatalı!" });
        }

        // Giriş başarılıysa bilgilerini dön (Bonus adımında buraya Token eklenecek)
        return Ok(new 
        { 
            success = true, 
            message = "Giriş başarılı", 
            data = new { user.Id, user.FullName, user.Role } 
        });
    }
}