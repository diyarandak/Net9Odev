namespace Net9Odev.DTOs;

// 1. Kullanıcıyı listelerken şifresini GİZLEMEK için bu paketi kullanacağız
public record UserResponseDto(
    int Id,
    string FullName,
    string Email,
    string Role,
    DateTime CreatedAt
);

// 2. Kayıt olurken isteyeceğimiz bilgiler
public record UserRegisterDto(
    string FullName,
    string Email,
    string Password,
    string Role = "User" // Varsayılan olarak "User" olsun
);

// 3. Giriş yaparken isteyeceğimiz bilgiler
public record UserLoginDto(
    string Email,
    string Password
);