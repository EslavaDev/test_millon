using Microsoft.AspNetCore.Mvc;
using RealEstate.Infrastructure.Data;

namespace RealEstate.Api.Controllers;

/// <summary>
/// Controller for database seeding operations (Development only)
/// </summary>
[ApiController]
[Route("api/seed")]
[Produces("application/json")]
public class SeedController : ControllerBase
{
    private readonly DatabaseSeeder _seeder;
    private readonly ILogger<SeedController> _logger;
    private readonly IHostEnvironment _environment;

    public SeedController(
        DatabaseSeeder seeder,
        ILogger<SeedController> logger,
        IHostEnvironment environment)
    {
        _seeder = seeder;
        _logger = logger;
        _environment = environment;
    }

    /// <summary>
    /// Seeds the database with sample data
    /// </summary>
    /// <param name="force">If true, clears existing data before seeding</param>
    /// <returns>Seed operation result</returns>
    /// <response code="200">Database seeded successfully</response>
    /// <response code="403">Forbidden in production environment</response>
    /// <response code="500">If seeding fails</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SeedDatabase([FromQuery] bool force = false)
    {
        // Only allow seeding in development environment
        if (!_environment.IsDevelopment())
        {
            _logger.LogWarning("Seed endpoint called in non-development environment");
            return StatusCode(403, new
            {
                error = "Forbidden",
                message = "Database seeding is only allowed in Development environment"
            });
        }

        try
        {
            _logger.LogInformation("Starting database seed (force: {Force})", force);

            await _seeder.SeedAsync(force);

            _logger.LogInformation("Database seeded successfully");

            return Ok(new
            {
                success = true,
                message = "Database seeded successfully",
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to seed database");
            return StatusCode(500, new
            {
                success = false,
                error = "Seeding failed",
                message = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets database statistics
    /// </summary>
    /// <returns>Count of documents in each collection</returns>
    /// <response code="200">Returns document counts</response>
    [HttpGet("stats")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetStats()
    {
        // This endpoint doesn't require Development environment
        // It's useful to check if data exists

        return Ok(new
        {
            message = "Use MongoDB queries or API endpoints to check data",
            endpoints = new
            {
                owners = "/api/owners (not implemented yet)",
                properties = "/api/properties",
                health = "/health"
            }
        });
    }
}
