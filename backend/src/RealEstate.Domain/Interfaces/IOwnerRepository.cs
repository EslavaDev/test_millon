using RealEstate.Domain.Entities;

namespace RealEstate.Domain.Interfaces;

/// <summary>
/// Repository interface for Owner entity operations.
/// </summary>
public interface IOwnerRepository
{
    /// <summary>
    /// Gets an owner by their unique identifier.
    /// </summary>
    /// <param name="id">The owner ID.</param>
    /// <returns>The owner, or null if not found.</returns>
    Task<Owner?> GetByIdAsync(string id);

    /// <summary>
    /// Gets all owners.
    /// </summary>
    /// <returns>A list of all owners.</returns>
    Task<List<Owner>> GetAllAsync();

    /// <summary>
    /// Adds a new owner to the repository.
    /// </summary>
    /// <param name="owner">The owner to add.</param>
    /// <returns>The added owner with its generated ID.</returns>
    Task<Owner> AddAsync(Owner owner);

    /// <summary>
    /// Updates an existing owner.
    /// </summary>
    /// <param name="owner">The owner to update.</param>
    /// <returns>True if the update was successful, false otherwise.</returns>
    Task<bool> UpdateAsync(Owner owner);

    /// <summary>
    /// Deletes an owner by their ID.
    /// </summary>
    /// <param name="id">The ID of the owner to delete.</param>
    /// <returns>True if the deletion was successful, false otherwise.</returns>
    Task<bool> DeleteAsync(string id);
}
