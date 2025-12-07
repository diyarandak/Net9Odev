using System.Net;
using System.Text.Json;
using Net9Odev.DTOs; // ApiResponse'u kullanmak için

namespace Net9Odev.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        // GÖRSELDEKİ FORMAT: Success, Message, Data (Null)
        var response = ApiResponse<object>.Fail("Sunucu hatası: " + exception.Message);

        var json = JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(json);
    }
}