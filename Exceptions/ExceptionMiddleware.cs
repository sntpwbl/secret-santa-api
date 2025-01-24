using Microsoft.AspNetCore.Http;
using SecretSanta.Exceptions;
using System.Net;
using System.Text.Json;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var statusCode = (int)HttpStatusCode.InternalServerError;
        var result = JsonSerializer.Serialize(new { error = "An unexpected error occurred." });

        if (exception is NotFoundException)
        {
            statusCode = (int)HttpStatusCode.NotFound;
            result = JsonSerializer.Serialize(new { error = exception.Message });
        }
        if (exception is InvalidPasswordException)
        {
            statusCode = (int)HttpStatusCode.Unauthorized;
            result = JsonSerializer.Serialize(new { error = exception.Message });
        }

        context.Response.StatusCode = statusCode;
        return context.Response.WriteAsync(result);
    }
}
