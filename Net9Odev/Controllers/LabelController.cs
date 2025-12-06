using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Net9Odev.Data;
using Net9Odev.DTOs;
using Net9Odev.Entities;

namespace Net9Odev.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LabelController : ControllerBase
{
    private readonly AppDbContext _context;

    public LabelController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var labels = await _context.Labels.ToListAsync();
        var dtos = labels.Select(l => new LabelResponseDto(l.Id, l.Name, l.Country, l.CreatedAt)).ToList();

        return Ok(new { success = true, message = "Plak şirketleri listelendi", data = dtos });
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateLabelDto request)
    {
        var newLabel = new Label { Name = request.Name, Country = request.Country };
        _context.Labels.Add(newLabel);
        await _context.SaveChangesAsync();

        return Ok(new { success = true, message = "Plak şirketi eklendi", data = new { newLabel.Id } });
    }
}