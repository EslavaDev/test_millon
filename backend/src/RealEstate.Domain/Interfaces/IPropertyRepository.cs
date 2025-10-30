using RealEstate.Domain.Entities;

namespace RealEstate.Domain.Interfaces;

/// <summary>
/// Repository interface for Property entity operations.
/// </summary>
public interface IPropertyRepository
{
    /// <summary>
    /// Gets a property by its unique identifier, including related data (owner, images, traces).
    /// </summary>
    /// <param name="id">The property ID.</param>
    /// <returns>The property with related data, or null if not found.</returns>
    Task<Property?> GetByIdAsync(string id);

    /// <summary>
    /// Gets a filtered, sorted, and paginated list of properties.
    /// </summary>
    /// <param name="filter">Filter criteria for searching properties.</param>
    /// <returns>A tuple containing the list of properties and the total count.</returns>
    Task<(List<Property> Properties, long TotalCount)> GetFilteredAsync(PropertyFilter filter);

    /// <summary>
    /// Adds a new property to the repository.
    /// </summary>
    /// <param name="property">The property to add.</param>
    /// <returns>The added property with its generated ID.</returns>
    Task<Property> AddAsync(Property property);

    /// <summary>
    /// Updates an existing property.
    /// </summary>
    /// <param name="property">The property to update.</param>
    /// <returns>True if the update was successful, false otherwise.</returns>
    Task<bool> UpdateAsync(Property property);

    /// <summary>
    /// Deletes a property by its ID.
    /// </summary>
    /// <param name="id">The ID of the property to delete.</param>
    /// <returns>True if the deletion was successful, false otherwise.</returns>
    Task<bool> DeleteAsync(string id);
}

/// <summary>
/// Filter criteria for property searches.
/// </summary>
public record PropertyFilter(
    string? Name = null,
    string? Address = null,
    decimal? MinPrice = null,
    decimal? MaxPrice = null,
    int? Year = null,
    string SortBy = "name",
    bool SortDescending = false,
    int PageNumber = 1,
    int PageSize = 10
);
