using System.Net;
using System.Text.Json;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using RealEstate.Api.Middleware;
using RealEstate.Api.Models;
using RealEstate.Domain.Exceptions;

namespace RealEstate.Api.Tests.Middleware;

[TestFixture]
public class ExceptionHandlingMiddlewareTests
{
    private Mock<ILogger<ExceptionHandlingMiddleware>> _mockLogger = null!;
    private Mock<IHostEnvironment> _mockEnvironment = null!;
    private DefaultHttpContext _httpContext = null!;

    [SetUp]
    public void SetUp()
    {
        _mockLogger = new Mock<ILogger<ExceptionHandlingMiddleware>>();
        _mockEnvironment = new Mock<IHostEnvironment>();
        _httpContext = new DefaultHttpContext();
        _httpContext.Response.Body = new MemoryStream();
    }

    [Test]
    public async Task InvokeAsync_WithNoException_ShouldCallNextDelegate()
    {
        // Arrange
        var nextCalled = false;
        RequestDelegate next = (HttpContext hc) =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        };

        var middleware = new ExceptionHandlingMiddleware(next, _mockLogger.Object, _mockEnvironment.Object);

        // Act
        await middleware.InvokeAsync(_httpContext);

        // Assert
        nextCalled.Should().BeTrue();
        _httpContext.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Test]
    public async Task InvokeAsync_WithEntityNotFoundException_ShouldReturn404()
    {
        // Arrange
        var exception = new EntityNotFoundException("Property with ID '123' was not found");

        RequestDelegate next = (HttpContext hc) => throw exception;

        _mockEnvironment.Setup(e => e.EnvironmentName).Returns(Environments.Development);

        var middleware = new ExceptionHandlingMiddleware(next, _mockLogger.Object, _mockEnvironment.Object);

        // Act
        await middleware.InvokeAsync(_httpContext);

        // Assert
        _httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        _httpContext.Response.ContentType.Should().Be("application/json");

        _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseBody, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        errorResponse.Should().NotBeNull();
        errorResponse!.Title.Should().Be("Resource Not Found");
        errorResponse.Status.Should().Be(404);
        errorResponse.Detail.Should().Contain("Property with ID '123' was not found");
    }

    [Test]
    public async Task InvokeAsync_WithDomainValidationException_ShouldReturn400()
    {
        // Arrange
        var errors = new Dictionary<string, string[]>
        {
            { "Name", new[] { "Name is required" } },
            { "Price", new[] { "Price must be greater than 0" } }
        };
        var exception = new Domain.Exceptions.ValidationException("Validation failed", errors);

        RequestDelegate next = (HttpContext hc) => throw exception;

        _mockEnvironment.Setup(e => e.EnvironmentName).Returns(Environments.Development);

        var middleware = new ExceptionHandlingMiddleware(next, _mockLogger.Object, _mockEnvironment.Object);

        // Act
        await middleware.InvokeAsync(_httpContext);

        // Assert
        _httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        _httpContext.Response.ContentType.Should().Be("application/json");

        _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseBody, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        errorResponse.Should().NotBeNull();
        errorResponse!.Title.Should().Be("Validation Error");
        errorResponse.Status.Should().Be(400);
        errorResponse.Detail.Should().Be("Validation failed");
        errorResponse.Errors.Should().NotBeNull();
        errorResponse.Errors.Should().ContainKey("Name");
        errorResponse.Errors.Should().ContainKey("Price");
    }

    [Test]
    public async Task InvokeAsync_WithFluentValidationException_ShouldReturn400()
    {
        // Arrange
        var validationFailures = new List<ValidationFailure>
        {
            new ValidationFailure("PageNumber", "Page number must be greater than 0"),
            new ValidationFailure("PageSize", "Page size must be between 1 and 100")
        };
        var exception = new FluentValidation.ValidationException(validationFailures);

        RequestDelegate next = (HttpContext hc) => throw exception;

        _mockEnvironment.Setup(e => e.EnvironmentName).Returns(Environments.Development);

        var middleware = new ExceptionHandlingMiddleware(next, _mockLogger.Object, _mockEnvironment.Object);

        // Act
        await middleware.InvokeAsync(_httpContext);

        // Assert
        _httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);

        _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseBody, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        errorResponse.Should().NotBeNull();
        errorResponse!.Title.Should().Be("Validation Error");
        errorResponse.Status.Should().Be(400);
        errorResponse.Detail.Should().Be("One or more validation errors occurred");
        errorResponse.Errors.Should().NotBeNull();
        errorResponse.Errors.Should().ContainKey("PageNumber");
        errorResponse.Errors.Should().ContainKey("PageSize");
    }

    [Test]
    public async Task InvokeAsync_WithDomainException_ShouldReturn400()
    {
        // Arrange
        var exception = new DomainException("Invalid operation in domain layer");

        RequestDelegate next = (HttpContext hc) => throw exception;

        _mockEnvironment.Setup(e => e.EnvironmentName).Returns(Environments.Development);

        var middleware = new ExceptionHandlingMiddleware(next, _mockLogger.Object, _mockEnvironment.Object);

        // Act
        await middleware.InvokeAsync(_httpContext);

        // Assert
        _httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);

        _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseBody, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        errorResponse.Should().NotBeNull();
        errorResponse!.Title.Should().Be("Domain Error");
        errorResponse.Status.Should().Be(400);
        errorResponse.Detail.Should().Be("Invalid operation in domain layer");
    }

    [Test]
    public async Task InvokeAsync_WithGenericException_InDevelopment_ShouldReturn500WithDetails()
    {
        // Arrange
        var exception = new InvalidOperationException("Something went wrong");

        RequestDelegate next = (HttpContext hc) => throw exception;

        _mockEnvironment.Setup(e => e.EnvironmentName).Returns(Environments.Development);

        var middleware = new ExceptionHandlingMiddleware(next, _mockLogger.Object, _mockEnvironment.Object);

        // Act
        await middleware.InvokeAsync(_httpContext);

        // Assert
        _httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

        _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseBody, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        errorResponse.Should().NotBeNull();
        errorResponse!.Title.Should().Be("Internal Server Error");
        errorResponse.Status.Should().Be(500);
        errorResponse.Detail.Should().Be("Something went wrong"); // Should show details in Development
    }

    [Test]
    public async Task InvokeAsync_WithGenericException_InProduction_ShouldReturn500WithoutDetails()
    {
        // Arrange
        var exception = new InvalidOperationException("Something went wrong");

        RequestDelegate next = (HttpContext hc) => throw exception;

        _mockEnvironment.Setup(e => e.EnvironmentName).Returns(Environments.Production);

        var middleware = new ExceptionHandlingMiddleware(next, _mockLogger.Object, _mockEnvironment.Object);

        // Act
        await middleware.InvokeAsync(_httpContext);

        // Assert
        _httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

        _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseBody, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        errorResponse.Should().NotBeNull();
        errorResponse!.Title.Should().Be("Internal Server Error");
        errorResponse.Status.Should().Be(500);
        errorResponse.Detail.Should().Be("An error occurred while processing your request"); // Generic message in Production
        errorResponse.Detail.Should().NotContain("Something went wrong");
    }

    [Test]
    public async Task InvokeAsync_WithException_ShouldLogError()
    {
        // Arrange
        var exception = new InvalidOperationException("Test exception");

        RequestDelegate next = (HttpContext hc) => throw exception;

        _mockEnvironment.Setup(e => e.EnvironmentName).Returns(Environments.Development);

        var middleware = new ExceptionHandlingMiddleware(next, _mockLogger.Object, _mockEnvironment.Object);

        // Act
        await middleware.InvokeAsync(_httpContext);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.Once);
    }

    [Test]
    public async Task InvokeAsync_ShouldSetCorrectContentType()
    {
        // Arrange
        var exception = new EntityNotFoundException("Test");

        RequestDelegate next = (HttpContext hc) => throw exception;

        _mockEnvironment.Setup(e => e.EnvironmentName).Returns(Environments.Development);

        var middleware = new ExceptionHandlingMiddleware(next, _mockLogger.Object, _mockEnvironment.Object);

        // Act
        await middleware.InvokeAsync(_httpContext);

        // Assert
        _httpContext.Response.ContentType.Should().Be("application/json");
    }

    [Test]
    public async Task InvokeAsync_ShouldIncludeTraceId()
    {
        // Arrange
        var exception = new EntityNotFoundException("Test");
        _httpContext.TraceIdentifier = "test-trace-id";

        RequestDelegate next = (HttpContext hc) => throw exception;

        _mockEnvironment.Setup(e => e.EnvironmentName).Returns(Environments.Development);

        var middleware = new ExceptionHandlingMiddleware(next, _mockLogger.Object, _mockEnvironment.Object);

        // Act
        await middleware.InvokeAsync(_httpContext);

        // Assert
        _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseBody, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        errorResponse.Should().NotBeNull();
        errorResponse!.TraceId.Should().Be("test-trace-id");
    }

    [Test]
    public async Task InvokeAsync_InDevelopment_ShouldReturnIndentedJson()
    {
        // Arrange
        var exception = new EntityNotFoundException("Test");

        RequestDelegate next = (HttpContext hc) => throw exception;

        _mockEnvironment.Setup(e => e.EnvironmentName).Returns(Environments.Development);

        var middleware = new ExceptionHandlingMiddleware(next, _mockLogger.Object, _mockEnvironment.Object);

        // Act
        await middleware.InvokeAsync(_httpContext);

        // Assert
        _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();

        // In development, JSON should be indented (contain newlines)
        responseBody.Should().Contain("\n");
    }

    [Test]
    public async Task InvokeAsync_WithMultipleValidationErrors_ShouldGroupByPropertyName()
    {
        // Arrange
        var validationFailures = new List<ValidationFailure>
        {
            new ValidationFailure("Name", "Name is required"),
            new ValidationFailure("Name", "Name must be at least 3 characters"),
            new ValidationFailure("Price", "Price must be greater than 0")
        };
        var exception = new FluentValidation.ValidationException(validationFailures);

        RequestDelegate next = (HttpContext hc) => throw exception;

        _mockEnvironment.Setup(e => e.EnvironmentName).Returns(Environments.Development);

        var middleware = new ExceptionHandlingMiddleware(next, _mockLogger.Object, _mockEnvironment.Object);

        // Act
        await middleware.InvokeAsync(_httpContext);

        // Assert
        _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseBody, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        errorResponse.Should().NotBeNull();
        errorResponse!.Errors.Should().NotBeNull();
        errorResponse.Errors.Should().ContainKey("Name");

        // Verify Name has 2 errors
        errorResponse.Errors!["Name"].Should().HaveCount(2);
        errorResponse.Errors["Name"].Should().Contain("Name is required");
        errorResponse.Errors["Name"].Should().Contain("Name must be at least 3 characters");
    }
}
