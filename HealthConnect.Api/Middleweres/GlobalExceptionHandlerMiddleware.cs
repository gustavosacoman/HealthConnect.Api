namespace HealthConnect.Api.Middleweres;

using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

/// <summary>
/// MIddelware to handle global exceptions and return appropriate HTTP responses.
/// </summary>
/// <param name="next">next to pass to anohter middleware and continue the pipeline. </param>
/// <param name="logger">logger to made a log cause.</param>
public class GlobalExceptionHandlerMiddleware(
    RequestDelegate next,
    ILogger<GlobalExceptionHandlerMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger = logger;

    /// <summary>
    /// InvokeAsync method to handle exceptions globally.
    /// </summary>
    /// <param name="context">context shows what is comming.</param>
    /// <returns>return void.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred while processing the request.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = exception switch
        {
            KeyNotFoundException _ => HttpStatusCode.NotFound,
            ArgumentNullException _ => HttpStatusCode.NotFound,
            ArgumentException _ => HttpStatusCode.BadRequest,
            InvalidOperationException _ => HttpStatusCode.NotAcceptable,
            NullReferenceException _ => HttpStatusCode.BadRequest,
            UnauthorizedAccessException _ => HttpStatusCode.Unauthorized,
            _ => HttpStatusCode.InternalServerError
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var errorResponse = new
        {
            StatusCode = context.Response.StatusCode,
            Message = statusCode == HttpStatusCode.InternalServerError ? "An unexpected error occurred." : exception.Message,
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
    }
}
