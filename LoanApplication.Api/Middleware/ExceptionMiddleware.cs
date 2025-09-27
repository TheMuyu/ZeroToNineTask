using LoanApplication.Api.Models;
using System.Net;
using System.Text.Json;

namespace LoanApplication.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var problem = new ProblemDetailsResponse
            {
                Type = exception switch
                {
                    ArgumentException => "https://tools.ietf.org/html/rfc7231#section-6.5.1", // Bad Request for argument errors
                    _ => "https://tools.ietf.org/html/rfc7231#section-5.1.1" // Internal Server Error default
                },
                Title = exception switch
                {
                    ArgumentException => "Validation Error",
                    _ => "An error occurred"
                },
                Status = exception switch
                {
                    ArgumentException => (int)HttpStatusCode.BadRequest,
                    _ => (int)HttpStatusCode.InternalServerError
                },
                Detail = exception.Message,
                Instance = context.Request.Path
            };

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(problem, options);

            context.Response.StatusCode = problem.Status;
            await context.Response.WriteAsync(json);
        }
    }
}