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

// AYARLAR
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

// SEED DATA
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated(); 
    await Net9Odev.Data.DataSeeder.SeedAsync(context);
}

// PIPELINE
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers(); // User, Album, Artist (Controller)

// ==========================================
// MINIMAL API BÖLGESİ (Concert, Song, Label)
// ==========================================

// --- A) CONCERT (Yeni Minimal API) ---
var concertGroup = app.MapGroup("/api/concert").WithTags("Concerts (Minimal API)");

concertGroup.MapGet("/", async (IConcertService service) => 
    Results.Ok(ApiResponse<object>.Ok(await service.GetAllAsync(), "Konserler listelendi")));

concertGroup.MapGet("/{id}", async (int id, IConcertService service) => {
    var result = await service.GetByIdAsync(id);
    return result is not null ? Results.Ok(ApiResponse<object>.Ok(result)) : Results.NotFound(ApiResponse<object>.Fail("Bulunamadı"));
});

concertGroup.MapPost("/", async (CreateConcertDto request, IConcertService service) => {
    try {
        var newId = await service.CreateAsync(request);
        return Results.Created($"/api/concert/{newId}", ApiResponse<object>.Ok(new { id = newId }, "Eklendi"));
    } catch (Exception ex) { return Results.BadRequest(ApiResponse<object>.Fail(ex.Message)); }
}).RequireAuthorization();

concertGroup.MapPut("/{id}", async (int id, UpdateConcertDto request, IConcertService service) => {
    return await service.UpdateAsync(id, request) 
        ? Results.Ok(ApiResponse<object>.Ok(null, "Güncellendi")) : Results.NotFound(ApiResponse<object>.Fail("Bulunamadı"));
}).RequireAuthorization();

concertGroup.MapDelete("/{id}", async (int id, IConcertService service) => {
    return await service.DeleteAsync(id) 
        ? Results.Ok(ApiResponse<object>.Ok(null, "Silindi")) : Results.NotFound(ApiResponse<object>.Fail("Bulunamadı"));
}).RequireAuthorization();


// --- B) SONG ---
var songGroup = app.MapGroup("/api/song").WithTags("Songs (Minimal API)");

songGroup.MapGet("/", async (ISongService service) => 
    Results.Ok(ApiResponse<object>.Ok(await service.GetAllAsync(), "Şarkılar listelendi")));

songGroup.MapGet("/{id}", async (int id, ISongService service) => {
    var result = await service.GetByIdAsync(id);
    return result is not null ? Results.Ok(ApiResponse<object>.Ok(result)) : Results.NotFound(ApiResponse<object>.Fail("Bulunamadı"));
});

songGroup.MapPost("/", async (CreateSongDto request, ISongService service) => {
    var newId = await service.CreateAsync(request);
    return Results.Created($"/api/song/{newId}", ApiResponse<object>.Ok(new { id = newId }, "Eklendi"));
}).RequireAuthorization();

songGroup.MapPut("/{id}", async (int id, UpdateSongDto request, ISongService service) => {
    return await service.UpdateAsync(id, request) 
        ? Results.Ok(ApiResponse<object>.Ok(null, "Güncellendi")) : Results.NotFound(ApiResponse<object>.Fail("Bulunamadı"));
}).RequireAuthorization();

songGroup.MapDelete("/{id}", async (int id, ISongService service) => {
    return await service.DeleteAsync(id) 
        ? Results.Ok(ApiResponse<object>.Ok(null, "Silindi")) : Results.NotFound(ApiResponse<object>.Fail("Bulunamadı"));
}).RequireAuthorization();


// --- C) LABEL ---
var labelGroup = app.MapGroup("/api/label").WithTags("Labels (Minimal API)");

labelGroup.MapGet("/", async (ILabelService service) => 
    Results.Ok(ApiResponse<object>.Ok(await service.GetAllAsync(), "Şirketler listelendi")));

labelGroup.MapGet("/{id}", async (int id, ILabelService service) => {
    var result = await service.GetByIdAsync(id);
    return result is not null ? Results.Ok(ApiResponse<object>.Ok(result)) : Results.NotFound(ApiResponse<object>.Fail("Bulunamadı"));
});

labelGroup.MapPost("/", async (CreateLabelDto request, ILabelService service) => {
    var newId = await service.CreateAsync(request);
    return Results.Created($"/api/label/{newId}", ApiResponse<object>.Ok(new { id = newId }, "Eklendi"));
}).RequireAuthorization();

labelGroup.MapPut("/{id}", async (int id, UpdateLabelDto request, ILabelService service) => {
    return await service.UpdateAsync(id, request) 
        ? Results.Ok(ApiResponse<object>.Ok(null, "Güncellendi")) : Results.NotFound(ApiResponse<object>.Fail("Bulunamadı"));
}).RequireAuthorization();

labelGroup.MapDelete("/{id}", async (int id, ILabelService service) => {
    return await service.DeleteAsync(id) 
        ? Results.Ok(ApiResponse<object>.Ok(null, "Silindi")) : Results.NotFound(ApiResponse<object>.Fail("Bulunamadı"));
}).RequireAuthorization();

app.Run();