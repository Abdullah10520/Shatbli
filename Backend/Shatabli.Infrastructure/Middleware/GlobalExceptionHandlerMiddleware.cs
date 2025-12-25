using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shatabli.Core.Domain.Exceptions;
using FluentValidation;
using System.Text.Json;

namespace Shatabli.Infrastructure.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
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
                _logger.LogError(ex, "An unexpected error occurred: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/problem+json";

            var problemDetails = new ProblemDetails
            {
                Instance = context.Request.Path
            };

            Dictionary<string, List<string>>? validationErrors = null;

            switch (exception)
            {
                case ValidationException fluentValidationEx:
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Title = "Validation Error";
                    problemDetails.Detail = "One or more validation errors occurred";
                    problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
                    
                    validationErrors = fluentValidationEx.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToList()
                        );
                    break;

                case AIProcessingException:
                    problemDetails.Status = StatusCodes.Status503ServiceUnavailable;
                    problemDetails.Title = "AI Service Error";
                    problemDetails.Detail = exception.Message;
                    problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.6.4";
                    break;

                case FileUploadException:
                    problemDetails.Status = StatusCodes.Status500InternalServerError;
                    problemDetails.Title = "File Upload Error";
                    problemDetails.Detail = exception.Message;
                    problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1";
                    break;

                case InvalidFileTypeException:
                    problemDetails.Status = StatusCodes.Status415UnsupportedMediaType;
                    problemDetails.Title = "Unsupported File Type";
                    problemDetails.Detail = exception.Message;
                    problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.13";
                    break;

                case ArgumentNullException:
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Title = "Bad Request";
                    problemDetails.Detail = "Required parameter is missing";
                    problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
                    break;

                case ArgumentException:
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Title = "Bad Request";
                    problemDetails.Detail = exception.Message;
                    problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
                    break;

                case InvalidOperationException:
                    problemDetails.Status = StatusCodes.Status500InternalServerError;
                    problemDetails.Title = "Operation Failed";
                    problemDetails.Detail = exception.Message;
                    problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1";
                    break;

                case UnauthorizedAccessException:
                    problemDetails.Status = StatusCodes.Status401Unauthorized;
                    problemDetails.Title = "Unauthorized";
                    problemDetails.Detail = exception.Message;
                    problemDetails.Type = "https://tools.ietf.org/html/rfc7235#section-3.1";
                    break;

                default:
                    problemDetails.Status = StatusCodes.Status500InternalServerError;
                    problemDetails.Title = "Internal Server Error";
                    problemDetails.Detail = "An unexpected error occurred. Please try again later";
                    problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1";
                    break;
            }

            context.Response.StatusCode = problemDetails.Status.Value;

            var errorResponse = new
            {
                type = problemDetails.Type,
                title = problemDetails.Title,
                status = problemDetails.Status,
                detail = problemDetails.Detail,
                instance = problemDetails.Instance,
                traceId = context.TraceIdentifier,
                errors = validationErrors
            };

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse, options));
        }
    }
}