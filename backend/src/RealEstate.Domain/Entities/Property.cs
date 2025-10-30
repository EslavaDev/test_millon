using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RealEstate.Domain.Entities;

/// <summary>
/// Represents a real estate property in the system.
/// </summary>
public class Property
{
    /// <summary>
    /// Unique identifier for the property (MongoDB ObjectId).
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string IdProperty { get; set; } = string.Empty;

    /// <summary>
    /// Name or title of the property.
    /// </summary>
    [BsonElement("name")]
    [BsonRequired]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Physical address of the property.
    /// </summary>
    [BsonElement("address")]
    [BsonRequired]
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Current price of the property.
    /// </summary>
    [BsonElement("price")]
    [BsonRequired]
    public decimal Price { get; set; }

    /// <summary>
    /// Internal code number for the property.
    /// </summary>
    [BsonElement("codeInternal")]
    public string? CodeInternal { get; set; }

    /// <summary>
    /// Year the property was built or established.
    /// </summary>
    [BsonElement("year")]
    public int Year { get; set; }

    /// <summary>
    /// Reference to the owner of this property (ObjectId as string).
    /// </summary>
    [BsonElement("idOwner")]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonRequired]
    public string IdOwner { get; set; } = string.Empty;

    /// <summary>
    /// Navigation property to the owner. Not stored in MongoDB, populated via aggregation.
    /// </summary>
    [BsonElement("Owner")]
    [BsonIgnoreIfNull]
    public Owner? Owner { get; set; }

    /// <summary>
    /// Navigation property to property images. Not stored in MongoDB, populated via aggregation.
    /// </summary>
    [BsonElement("Images")]
    [BsonIgnoreIfDefault]
    public List<PropertyImage> Images { get; set; } = new();

    /// <summary>
    /// Navigation property to property traces (sale history). Not stored in MongoDB, populated via aggregation.
    /// </summary>
    [BsonElement("Traces")]
    [BsonIgnoreIfDefault]
    public List<PropertyTrace> Traces { get; set; } = new();

    /// <summary>
    /// Date and time when the property record was created.
    /// </summary>
    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Date and time when the property record was last updated.
    /// </summary>
    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
