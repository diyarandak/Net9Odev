namespace Net9Odev.DTOs;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T? Data { get; set; }

    // Başarılı Cevap İçin Helper
    public static ApiResponse<T> Ok(T data, string message = "İşlem başarılı")
    {
        return new ApiResponse<T> { Success = true, Message = message, Data = data };
    }

    // Hata Cevabı İçin Helper
    public static ApiResponse<T> Fail(string message)
    {
        return new ApiResponse<T> { Success = false, Message = message, Data = default };
    }
}