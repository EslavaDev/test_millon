using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.DTOs;
using RealEstate.Application.Interfaces;

namespace RealEstate.Api.Controllers;

/// <summary>
/// Controller for managing real estate properties
/// </summary>
[ApiController]
[Route("api/properties")]
[Produces("application/json")]
public class PropertiesController : ControllerBase
{
    private readonly IPropertyService _propertyService;
    private readonly IValidator<PropertyFilterDto> _filterValidator;
    private readonly ILogger<PropertiesController> _logger;

    public PropertiesController(
        IPropertyService propertyService,
        IValidator<PropertyFilterDto> filterValidator,
        ILogger<PropertiesController> logger)
    {
        _propertyService = propertyService;
        _filterValidator = filterValidator;
        _logger = logger;
    }

    /// <summary>
    /// Get a paginated list of properties with optional filtering
    /// </summary>
    /// <param name="filter">Filter criteria for properties</param>
    /// <returns>Paginated list of properties</returns>
    /// <response code="200">Returns the paginated list of properties</response>
    /// <response code="400">If the filter parameters are invalid</response>
    /// <response code="500">If an internal server error occurs</response>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResultDto<PropertyListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PagedResultDto<PropertyListDto>>> GetProperties(
        [FromQuery] PropertyFilterDto filter)
    {
        _logger.LogInformation("Getting properties with filter: {@Filter}", filter);

        // Validate filter
        var validationResult = await _filterValidator.ValidateAsync(filter);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Invalid filter parameters: {Errors}", validationResult.Errors);

            var errors = validationResult.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );

            return ValidationProblem(new ValidationProblemDetails(errors));
        }

        try
        {
            var result = await _propertyService.GetPropertiesAsync(filter);

            _logger.LogInformation(
                "Retrieved {Count} properties out of {Total} total",
                result.Items.Count,
                result.TotalCount);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving properties");
            throw;
        }
    }

    /// <summary>
    /// Get detailed information about a specific property
    /// </summary>
    /// <param name="id">Property identifier</param>
    /// <returns>Property details</returns>
    /// <response code="200">Returns the property details</response>
    /// <response code="404">If the property is not found</response>
    /// <response code="500">If an internal server error occurs</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PropertyDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PropertyDetailDto>> GetPropertyById(string id)
    {
        _logger.LogInformation("Getting property with ID: {PropertyId}", id);

        if (string.IsNullOrWhiteSpace(id))
        {
            _logger.LogWarning("Empty property ID provided");
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid Property ID",
                Detail = "Property ID cannot be empty",
                Status = StatusCodes.Status400BadRequest
            });
        }

        try
        {
            var property = await _propertyService.GetPropertyByIdAsync(id);

            if (property == null)
            {
                _logger.LogWarning("Property not found: {PropertyId}", id);
                return NotFound(new ProblemDetails
                {
                    Title = "Property Not Found",
                    Detail = $"Property with ID '{id}' was not found",
                    Status = StatusCodes.Status404NotFound
                });
            }

            _logger.LogInformation("Retrieved property: {PropertyName}", property.Name);
            return Ok(property);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving property {PropertyId}", id);
            throw;
        }
    }
}
