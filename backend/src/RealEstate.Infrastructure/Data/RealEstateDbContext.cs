using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RealEstate.Domain.Entities;
using RealEstate.Infrastructure.Configuration;

namespace RealEstate.Infrastructure.Data;

public class RealEstateDbContext
{
    private readonly IMongoDatabase _database;
    private readonly MongoDbSettings _settings;

    public RealEstateDbContext(IOptions<MongoDbSettings> settings)
    {
        _settings = settings.Value;
        var client = new MongoClient(_settings.ConnectionString);
        _database = client.GetDatabase(_settings.DatabaseName);
    }

    public IMongoCollection<Owner> Owners =>
        _database.GetCollection<Owner>(_settings.OwnersCollectionName);

    public IMongoCollection<Property> Properties =>
        _database.GetCollection<Property>(_settings.PropertiesCollectionName);

    public IMongoCollection<PropertyImage> PropertyImages =>
        _database.GetCollection<PropertyImage>(_settings.PropertyImagesCollectionName);

    public IMongoCollection<PropertyTrace> PropertyTraces =>
        _database.GetCollection<PropertyTrace>(_settings.PropertyTracesCollectionName);

    public async Task CreateIndexesAsync()
    {
        // Owner indexes
        var ownerIndexKeys = Builders<Owner>.IndexKeys.Ascending(o => o.Name);
        await Owners.Indexes.CreateOneAsync(new CreateIndexModel<Owner>(ownerIndexKeys));

        // Property indexes
        var propertyNameIndex = Builders<Property>.IndexKeys.Ascending(p => p.Name);
        await Properties.Indexes.CreateOneAsync(new CreateIndexModel<Property>(propertyNameIndex));

        var propertyAddressIndex = Builders<Property>.IndexKeys.Ascending(p => p.Address);
        await Properties.Indexes.CreateOneAsync(new CreateIndexModel<Property>(propertyAddressIndex));

        var propertyPriceIndex = Builders<Property>.IndexKeys.Ascending(p => p.Price);
        await Properties.Indexes.CreateOneAsync(new CreateIndexModel<Property>(propertyPriceIndex));

        var propertyYearIndex = Builders<Property>.IndexKeys.Ascending(p => p.Year);
        await Properties.Indexes.CreateOneAsync(new CreateIndexModel<Property>(propertyYearIndex));

        var propertyOwnerIndex = Builders<Property>.IndexKeys.Ascending(p => p.IdOwner);
        await Properties.Indexes.CreateOneAsync(new CreateIndexModel<Property>(propertyOwnerIndex));

        var propertyCodeInternalIndex = Builders<Property>.IndexKeys.Ascending(p => p.CodeInternal);
        await Properties.Indexes.CreateOneAsync(new CreateIndexModel<Property>(propertyCodeInternalIndex));

        // PropertyImage indexes
        var imagePropertyIndex = Builders<PropertyImage>.IndexKeys.Ascending(pi => pi.IdProperty);
        await PropertyImages.Indexes.CreateOneAsync(new CreateIndexModel<PropertyImage>(imagePropertyIndex));

        var imageEnabledIndex = Builders<PropertyImage>.IndexKeys.Ascending(pi => pi.Enabled);
        await PropertyImages.Indexes.CreateOneAsync(new CreateIndexModel<PropertyImage>(imageEnabledIndex));

        // PropertyTrace indexes
        var tracePropertyIndex = Builders<PropertyTrace>.IndexKeys.Ascending(pt => pt.IdProperty);
        await PropertyTraces.Indexes.CreateOneAsync(new CreateIndexModel<PropertyTrace>(tracePropertyIndex));

        var traceDateIndex = Builders<PropertyTrace>.IndexKeys.Descending(pt => pt.DateSale);
        await PropertyTraces.Indexes.CreateOneAsync(new CreateIndexModel<PropertyTrace>(traceDateIndex));
    }
}
