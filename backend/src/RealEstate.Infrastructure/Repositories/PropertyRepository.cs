using MongoDB.Bson;
using MongoDB.Driver;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Infrastructure.Data;

namespace RealEstate.Infrastructure.Repositories;

public class PropertyRepository : IPropertyRepository
{
    private readonly RealEstateDbContext _context;

    public PropertyRepository(RealEstateDbContext context)
    {
        _context = context;
    }

    public async Task<Property?> GetByIdAsync(string id)
    {
        if (!ObjectId.TryParse(id, out _))
        {
            return null;
        }

        var pipeline = new List<BsonDocument>
        {
            new BsonDocument("$match", new BsonDocument("_id", new ObjectId(id)))
        };
        pipeline.AddRange(GetLookupStages());

        var result = await _context.Properties
            .Aggregate<Property>(PipelineDefinition<Property, Property>.Create(pipeline))
            .FirstOrDefaultAsync();

        return result;
    }

    public async Task<(List<Property> Properties, long TotalCount)> GetFilteredAsync(PropertyFilter filter)
    {
        var matchConditions = new List<BsonDocument>();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            matchConditions.Add(new BsonDocument("name",
                new BsonDocument("$regex", new BsonRegularExpression(filter.Name, "i"))));
        }

        if (!string.IsNullOrWhiteSpace(filter.Address))
        {
            matchConditions.Add(new BsonDocument("address",
                new BsonDocument("$regex", new BsonRegularExpression(filter.Address, "i"))));
        }

        if (filter.MinPrice.HasValue)
        {
            matchConditions.Add(new BsonDocument("price",
                new BsonDocument("$gte", filter.MinPrice.Value)));
        }

        if (filter.MaxPrice.HasValue)
        {
            matchConditions.Add(new BsonDocument("price",
                new BsonDocument("$lte", filter.MaxPrice.Value)));
        }

        if (filter.Year.HasValue)
        {
            matchConditions.Add(new BsonDocument("year", filter.Year.Value));
        }

        // Build match stage
        var matchStage = matchConditions.Count > 0
            ? new BsonDocument("$match", new BsonDocument("$and", new BsonArray(matchConditions)))
            : new BsonDocument("$match", new BsonDocument());

        // Get total count
        var countPipeline = new[] { matchStage };
        var totalCount = await _context.Properties
            .Aggregate<Property>(PipelineDefinition<Property, Property>.Create(countPipeline))
            .ToListAsync();

        // Build sort stage
        var sortDirection = filter.SortDescending ? -1 : 1;
        var sortField = filter.SortBy.ToLower() switch
        {
            "name" => "name",
            "address" => "address",
            "price" => "price",
            "year" => "year",
            _ => "name"
        };
        var sortStage = new BsonDocument("$sort", new BsonDocument(sortField, sortDirection));

        // Build pagination stages
        var skip = (filter.PageNumber - 1) * filter.PageSize;
        var skipStage = new BsonDocument("$skip", skip);
        var limitStage = new BsonDocument("$limit", filter.PageSize);

        // Build complete pipeline
        var pipeline = new List<BsonDocument> { matchStage };
        pipeline.AddRange(GetLookupStages());
        pipeline.Add(sortStage);
        pipeline.Add(skipStage);
        pipeline.Add(limitStage);

        var properties = await _context.Properties
            .Aggregate<Property>(PipelineDefinition<Property, Property>.Create(pipeline))
            .ToListAsync();

        return (properties, totalCount.Count);
    }

    public async Task<Property> AddAsync(Property property)
    {
        property.CreatedAt = DateTime.UtcNow;
        property.UpdatedAt = DateTime.UtcNow;
        await _context.Properties.InsertOneAsync(property);
        return property;
    }

    public async Task<bool> UpdateAsync(Property property)
    {
        if (!ObjectId.TryParse(property.IdProperty, out _))
        {
            return false;
        }

        property.UpdatedAt = DateTime.UtcNow;

        var filter = Builders<Property>.Filter.Eq(p => p.IdProperty, property.IdProperty);
        var result = await _context.Properties.ReplaceOneAsync(filter, property);

        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        if (!ObjectId.TryParse(id, out _))
        {
            return false;
        }

        var filter = Builders<Property>.Filter.Eq(p => p.IdProperty, id);
        var result = await _context.Properties.DeleteOneAsync(filter);

        return result.IsAcknowledged && result.DeletedCount > 0;
    }

    private static BsonDocument[] GetLookupStages()
    {
        return new[]
        {
            // Lookup Owner - Compare ObjectIds directly
            new BsonDocument("$lookup", new BsonDocument
            {
                { "from", "Owners" },
                { "let", new BsonDocument("ownerId", "$idOwner") },
                { "pipeline", new BsonArray
                    {
                        new BsonDocument("$match", new BsonDocument("$expr",
                            new BsonDocument("$eq", new BsonArray
                            {
                                "$_id",
                                "$$ownerId"
                            })
                        )),
                        new BsonDocument("$limit", 1)
                    }
                },
                { "as", "Owner" }
            }),

            // Unwind Owner
            new BsonDocument("$unwind", new BsonDocument
            {
                { "path", "$Owner" },
                { "preserveNullAndEmptyArrays", true }
            }),

            // Lookup Images - Compare ObjectIds directly
            new BsonDocument("$lookup", new BsonDocument
            {
                { "from", "PropertyImages" },
                { "let", new BsonDocument("propertyId", "$_id") },
                { "pipeline", new BsonArray
                    {
                        new BsonDocument("$match", new BsonDocument("$expr",
                            new BsonDocument("$eq", new BsonArray { "$idProperty", "$$propertyId" })
                        ))
                    }
                },
                { "as", "Images" }
            }),

            // Lookup Traces - Compare ObjectIds directly
            new BsonDocument("$lookup", new BsonDocument
            {
                { "from", "PropertyTraces" },
                { "let", new BsonDocument("propertyId", "$_id") },
                { "pipeline", new BsonArray
                    {
                        new BsonDocument("$match", new BsonDocument("$expr",
                            new BsonDocument("$eq", new BsonArray { "$idProperty", "$$propertyId" })
                        )),
                        new BsonDocument("$sort", new BsonDocument("dateSale", -1))
                    }
                },
                { "as", "Traces" }
            })
        };
    }
}
