namespace RealEstate.Domain.Exceptions;

/// <summary>
/// Exception thrown when an entity is not found in the repository.
/// </summary>
public class EntityNotFoundException : DomainException
{
    public EntityNotFoundException()
    {
    }

    public EntityNotFoundException(string entityName, string entityId)
        : base($"{entityName} with ID '{entityId}' was not found.")
    {
        EntityName = entityName;
        EntityId = entityId;
    }

    public EntityNotFoundException(string message) : base(message)
    {
    }

    public EntityNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public string? EntityName { get; }
    public string? EntityId { get; }
}
