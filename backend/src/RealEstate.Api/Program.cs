using System.Reflection;
using RealEstate.Api.Middleware;
using RealEstate.Application;
using RealEstate.Infrastructure;

// Load environment variables from .env file
// Search for .env file in current directory and parent directories
string? FindEnvFile()
{
    var currentDir = Directory.GetCurrentDirectory();
    var searchPaths = new[]
    {
        currentDir,
        Path.Combine(currentDir, ".."),
        Path.Combine(currentDir, "..", ".."),
        Path.Combine(currentDir, "..", "..", ".."),
    };

    foreach (var path in searchPaths)
    {
        var envFile = Path.Combine(path, ".env");
        if (File.Exists(envFile))
        {
            return Path.GetFullPath(envFile);
        }
    }
    return null;
}

var envFilePath = FindEnvFile();
if (envFilePath != null)
{
    DotNetEnv.Env.Load(envFilePath);
    Console.WriteLine($"✓ Loaded .env from: {envFilePath}");
}
else
{
    Console.WriteLine("⚠ Warning: .env file not found");
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Add Controllers
builder.Services.AddControllers();

// Configure CORS
builder.Services.AddCors(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        // Development: Allow all origins for easy testing
        options.AddDefaultPolicy(policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
    }
    else
    {
        // Production: Only allow specific origins
        var corsOrigins = builder.Configuration["CorsOrigins"]?.Split(',') ?? new[] { "http://localhost:3000" };
        options.AddDefaultPolicy(policy =>
        {
            policy.WithOrigins(corsOrigins)
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
    }
});

// Add API Explorer for Swagger
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger with XML comments
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Real Estate Property Management API",
        Version = "v1",
        Description = "API for managing real estate properties, owners, and property information",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Development Team",
            Email = "dev@realestate.com"
        }
    });

    // Include XML comments
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

// Initialize database indexes (skip in test environment)
var initializeDatabase = builder.Configuration.GetValue<string>("InitializeDatabase");
if (initializeDatabase != "false")
{
    try
    {
        await app.Services.InitializeDatabaseAsync();
        Console.WriteLine("Database indexes initialized successfully");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Warning: Failed to initialize database indexes: {ex.Message}");
        Console.WriteLine("The API will still start, but database operations may fail.");
    }
}

// Configure the HTTP request pipeline.

// Exception handling middleware (must be first)
app.UseExceptionHandlingMiddleware();

// CORS - Must come before UseHttpsRedirection and UseRouting
app.UseCors();

// HTTPS Redirection (only in production)
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// Routing
app.UseRouting();

// Swagger (Development only)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Real Estate API V1");
        c.RoutePrefix = "swagger";
    });
}

// Authorization (for future use)
// app.UseAuthorization();

// Map Controllers
app.MapControllers();

// Health Check endpoint
app.MapGet("/health", () =>
{
    return Results.Ok(new
    {
        status = "Healthy",
        timestamp = DateTime.UtcNow,
        service = "Real Estate Property Management API",
        version = "1.0.0"
    });
})
.WithName("HealthCheck")
.WithTags("Health")
.Produces(200);

app.Run();

// Make the implicit Program class public for testing
public partial class Program { }
