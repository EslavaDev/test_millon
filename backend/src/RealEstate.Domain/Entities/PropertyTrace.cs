using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RealEstate.Domain.Entities;

/// <summary>
/// Represents a transaction or price history trace for a property.
/// </summary>
public class PropertyTrace
{
    /// <summary>
    /// Unique identifier for the property trace (MongoDB ObjectId).
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string IdPropertyTrace { get; set; } = string.Empty;

    /// <summary>
    /// Reference to the property this trace belongs to (ObjectId as string).
    /// </summary>
    [BsonElement("idProperty")]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonRequired]
    public string IdProperty { get; set; } = string.Empty;

    /// <summary>
    /// Date of the sale or transaction.
    /// </summary>
    [BsonElement("dateSale")]
    [BsonRequired]
    public DateTime DateSale { get; set; }

    /// <summary>
    /// Name of the entity or person involved in the transaction.
    /// </summary>
    [BsonElement("name")]
    [BsonRequired]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Sale price or transaction value.
    /// </summary>
    [BsonElement("value")]
    [BsonRequired]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Value { get; set; }

    /// <summary>
    /// Tax amount associated with the transaction.
    /// </summary>
    [BsonElement("tax")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Tax { get; set; }

    /// <summary>
    /// Date and time when the trace record was created.
    /// </summary>
    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Date and time when the trace record was last updated.
    /// </summary>
    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
