using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RealEstate.Domain.Entities;

/// <summary>
/// Represents a property owner in the real estate system.
/// </summary>
public class Owner
{
    /// <summary>
    /// Unique identifier for the owner (MongoDB ObjectId).
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string IdOwner { get; set; } = string.Empty;

    /// <summary>
    /// Full name of the owner.
    /// </summary>
    [BsonElement("name")]
    [BsonRequired]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Address of the owner.
    /// </summary>
    [BsonElement("address")]
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Photo URL or path for the owner.
    /// </summary>
    [BsonElement("photo")]
    public string? Photo { get; set; }

    /// <summary>
    /// Date of birth of the owner.
    /// </summary>
    [BsonElement("birthday")]
    public DateTime Birthday { get; set; }

    /// <summary>
    /// Date and time when the owner record was created.
    /// </summary>
    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Date and time when the owner record was last updated.
    /// </summary>
    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
