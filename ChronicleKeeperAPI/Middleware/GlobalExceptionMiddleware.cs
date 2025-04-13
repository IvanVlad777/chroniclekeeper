using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace ChronicleKeeperAPI.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "DB update exception occurred.");

                string message = "There was a database error.";

                if (dbEx.InnerException?.Message.Contains("FOREIGN KEY") == true)
                {
                    message = "You tried to reference an entity that does not exist (e.g. wrong NationId, ReligionId, etc.).";
                }

                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = message }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled error.");

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = "An unexpected error occurred." }));
            }
        }
    }
}
