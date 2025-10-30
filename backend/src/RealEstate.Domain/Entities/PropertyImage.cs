using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RealEstate.Domain.Entities;

/// <summary>
/// Represents an image associated with a property.
/// </summary>
public class PropertyImage
{
    /// <summary>
    /// Unique identifier for the property image (MongoDB ObjectId).
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string IdPropertyImage { get; set; } = string.Empty;

    /// <summary>
    /// Reference to the property this image belongs to (ObjectId as string).
    /// </summary>
    [BsonElement("idProperty")]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonRequired]
    public string IdProperty { get; set; } = string.Empty;

    /// <summary>
    /// URL or file path to the image.
    /// </summary>
    [BsonElement("file")]
    [BsonRequired]
    public string File { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether this image is enabled/active.
    /// </summary>
    [BsonElement("enabled")]
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Date and time when the image record was created.
    /// </summary>
    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Date and time when the image record was last updated.
    /// </summary>
    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
