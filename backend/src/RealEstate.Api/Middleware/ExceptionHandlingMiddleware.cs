using System.Net;
using System.Text.Json;
using FluentValidation;
using RealEstate.Api.Models;
using RealEstate.Domain.Exceptions;

namespace RealEstate.Api.Middleware;

/// <summary>
/// Middleware for handling exceptions globally
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = exception switch
        {
            EntityNotFoundException notFoundEx => new ErrorResponse
            {
                Title = "Resource Not Found",
                Status = (int)HttpStatusCode.NotFound,
                Detail = notFoundEx.Message,
                TraceId = context.TraceIdentifier
            },

            Domain.Exceptions.ValidationException validationEx => new ErrorResponse
            {
                Title = "Validation Error",
                Status = (int)HttpStatusCode.BadRequest,
                Detail = validationEx.Message,
                TraceId = context.TraceIdentifier,
                Errors = validationEx.Errors
            },

            FluentValidation.ValidationException fluentValidationEx => new ErrorResponse
            {
                Title = "Validation Error",
                Status = (int)HttpStatusCode.BadRequest,
                Detail = "One or more validation errors occurred",
                TraceId = context.TraceIdentifier,
                Errors = fluentValidationEx.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    )
            },

            DomainException domainEx => new ErrorResponse
            {
                Title = "Domain Error",
                Status = (int)HttpStatusCode.BadRequest,
                Detail = domainEx.Message,
                TraceId = context.TraceIdentifier
            },

            _ => new ErrorResponse
            {
                Title = "Internal Server Error",
                Status = (int)HttpStatusCode.InternalServerError,
                Detail = _environment.IsDevelopment()
                    ? exception.Message
                    : "An error occurred while processing your request",
                TraceId = context.TraceIdentifier
            }
        };

        context.Response.StatusCode = response.Status;

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = _environment.IsDevelopment()
        };

        var json = JsonSerializer.Serialize(response, options);
        await context.Response.WriteAsync(json);
    }
}

/// <summary>
/// Extension method to add exception handling middleware
/// </summary>
public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandlingMiddleware(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
