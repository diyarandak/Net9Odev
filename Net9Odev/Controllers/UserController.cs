using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Net9Odev.DTOs;
using Net9Odev.Services;

namespace Net9Odev.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(new { success = true, data = await _userService.GetAllAsync() });

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterDto request)
    {
        try { return Ok(new { success = true, message = "Kayıt başarılı", id = await _userService.RegisterAsync(request) }); }
        catch (Exception ex) { return Conflict(new { success = false, message = ex.Message }); }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginDto request)
    {
        var token = await _userService.LoginAsync(request);
        if (token == null) return Unauthorized(new { success = false, message = "E-posta veya şifre hatalı" });
        return Ok(new { success = true, message = "Giriş başarılı", data = new { Token = token } });
    }

    // YENİ EKLENENLER
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateUserDto request)
    {
        return await _userService.UpdateAsync(id, request) ? Ok(new { success = true, message = "Kullanıcı güncellendi" }) : NotFound();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        return await _userService.DeleteAsync(id) ? Ok(new { success = true, message = "Kullanıcı silindi" }) : NotFound();
    }
}