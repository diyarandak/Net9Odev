using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Net9Odev.Data;
using Net9Odev.DTOs;
using Net9Odev.Services;
using Net9Odev.Middleware;

var builder = WebApplication.CreateBuilder(args);

// 1. AYARLAR
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Servisler
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IArtistService, ArtistService>();
builder.Services.AddScoped<IAlbumService, AlbumService>();
builder.Services.AddScoped<ISongService, SongService>();
builder.Services.AddScoped<ILabelService, LabelService>();
builder.Services.AddScoped<IConcertService, ConcertService>();

// Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Net9Odev API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization", Type = SecuritySchemeType.ApiKey, Scheme = "Bearer", In = ParameterLocation.Header, Description = "Bearer {token}"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, Array.Empty<string>() }
    });
});

// JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, ValidateAudience = true, ValidateLifetime = true, ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"], ValidAudience = jwtSettings["Audience"], IssuerSigningKey = new SymmetricSecurityKey(secretKey)
        };
    });

var app = builder.Build();

// 2. PIPELINE
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ★ HATA YAKALAMA VE FORMATLAMA ★
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// 3. MINIMAL API (FORMATLI CEVAPLAR)
var artistGroup = app.MapGroup("/api/artists").WithTags("Artists (Minimal API Example)");

// GET (Listeleme)
artistGroup.MapGet("/", async (IArtistService service) => 
{
    var data = await service.GetAllArtistsAsync();
    return Results.Ok(ApiResponse<object>.Ok(data, "Sanatçılar listelendi"));
});

// GET (Detay)
artistGroup.MapGet("/{id}", async (int id, IArtistService service) => {
    var result = await service.GetArtistByIdAsync(id);
    return result is not null 
        ? Results.Ok(ApiResponse<object>.Ok(result)) 
        : Results.NotFound(ApiResponse<object>.Fail("Bulunamadı"));
});

// POST
artistGroup.MapPost("/", async (CreateArtistDto request, IArtistService service) => {
    var newId = await service.AddArtistAsync(request);
    // Created dönüşünde de formatı koruyoruz
    return Results.Created($"/api/artists/{newId}", ApiResponse<object>.Ok(new { id = newId }, "Sanatçı eklendi"));
}).RequireAuthorization();

// PUT
artistGroup.MapPut("/{id}", async (int id, UpdateArtistDto request, IArtistService service) => {
    return await service.UpdateArtistAsync(id, request) 
        ? Results.Ok(ApiResponse<object>.Ok(null, "Güncellendi")) 
        : Results.NotFound(ApiResponse<object>.Fail("Bulunamadı"));
}).RequireAuthorization();

// DELETE
artistGroup.MapDelete("/{id}", async (int id, IArtistService service) => {
    return await service.DeleteArtistAsync(id) 
        ? Results.Ok(ApiResponse<object>.Ok(null, "Silindi")) // NoContent yerine Ok dönüp mesaj gösteriyoruz
        : Results.NotFound(ApiResponse<object>.Fail("Bulunamadı"));
}).RequireAuthorization();

app.Run();