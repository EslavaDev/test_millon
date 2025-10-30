using RealEstate.Application.DTOs;

namespace RealEstate.Application.Interfaces;

/// <summary>
/// Service for managing property operations
/// </summary>
public interface IPropertyService
{
    /// <summary>
    /// Gets a paginated list of properties with optional filtering
    /// </summary>
    /// <param name="filter">Filter criteria for properties</param>
    /// <returns>Paginated result of property list items</returns>
    Task<PagedResultDto<PropertyListDto>> GetPropertiesAsync(PropertyFilterDto filter);

    /// <summary>
    /// Gets detailed information about a specific property
    /// </summary>
    /// <param name="id">Property identifier</param>
    /// <returns>Property details or null if not found</returns>
    Task<PropertyDetailDto?> GetPropertyByIdAsync(string id);
}
