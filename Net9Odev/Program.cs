using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Net9Odev.Data;
using Net9Odev.DTOs;
using Net9Odev.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. AYARLAR
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers(); // Diğerleri (Album, User) için Controller açık
builder.Services.AddEndpointsApiExplorer();

// Tüm Servisleri Tanıtıyoruz
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IArtistService, ArtistService>();
builder.Services.AddScoped<IAlbumService, AlbumService>();
builder.Services.AddScoped<ISongService, SongService>();
builder.Services.AddScoped<ILabelService, LabelService>();
builder.Services.AddScoped<IConcertService, ConcertService>();

// Swagger ve Kilit Ayarı
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Net9Odev API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        In = ParameterLocation.Header,
        Description = "Bearer {token}"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, Array.Empty<string>() }
    });
});

// JWT Şifre Ayarı
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(secretKey)
        };
    });

var app = builder.Build();

// 2. ÇALIŞMA AYARLARI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers(); // Album, User vb. buradan çalışacak

// 3. ARTIST İÇİN MINIMAL API (Hocanın Özel İsteği)
// ArtistController'ı sildik, onun yerine burası çalışacak.
var artistGroup = app.MapGroup("/api/artists").WithTags("Artists (Minimal API Example)");

artistGroup.MapGet("/", async (IArtistService service) => Results.Ok(await service.GetAllArtistsAsync()));
artistGroup.MapGet("/{id}", async (int id, IArtistService service) => {
    var result = await service.GetArtistByIdAsync(id);
    return result is not null ? Results.Ok(result) : Results.NotFound();
});
artistGroup.MapPost("/", async (CreateArtistDto request, IArtistService service) => {
    var newId = await service.AddArtistAsync(request);
    return Results.Created($"/api/artists/{newId}", new { id = newId });
}).RequireAuthorization(); // Kilitli
artistGroup.MapPut("/{id}", async (int id, UpdateArtistDto request, IArtistService service) => {
    return await service.UpdateArtistAsync(id, request) ? Results.Ok("Güncellendi") : Results.NotFound();
}).RequireAuthorization(); // Kilitli
artistGroup.MapDelete("/{id}", async (int id, IArtistService service) => {
    return await service.DeleteArtistAsync(id) ? Results.NoContent() : Results.NotFound();
}).RequireAuthorization(); // Kilitli

app.Run();