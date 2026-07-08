using ChronicleKeeper.Core.Exceptions;
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
            catch (EntityNotFoundException nfex)
            {
                _logger.LogWarning("Entity not found: {Message}", nfex.Message);
                await WriteError(context, HttpStatusCode.NotFound, nfex.Message);
            }
            catch (ForbiddenAccessException fex)
            {
                _logger.LogWarning("Forbidden access: {Message}", fex.Message);
                await WriteError(context, HttpStatusCode.Forbidden, fex.Message);
            }
            catch (DomainValidationException vex)
            {
                _logger.LogWarning(vex, "Domain validation failed.");
                await WriteError(context, HttpStatusCode.BadRequest, vex.Message);
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "DB update exception occurred.");

                string message = "There was a database error.";

                if (dbEx.InnerException?.Message.Contains("FOREIGN KEY") == true)
                {
                    message = "You tried to reference an entity that does not exist (e.g. wrong NationId, ReligionId, etc.).";
                }

                await WriteError(context, HttpStatusCode.BadRequest, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled error.");
                await WriteError(context, HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }
        }

        private static async Task WriteError(HttpContext context, HttpStatusCode statusCode, string message)
        {
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = message }));
        }
    }
}
