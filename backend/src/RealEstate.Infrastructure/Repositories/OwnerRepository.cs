using MongoDB.Bson;
using MongoDB.Driver;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Infrastructure.Data;

namespace RealEstate.Infrastructure.Repositories;

public class OwnerRepository : IOwnerRepository
{
    private readonly RealEstateDbContext _context;

    public OwnerRepository(RealEstateDbContext context)
    {
        _context = context;
    }

    public async Task<Owner?> GetByIdAsync(string id)
    {
        if (!ObjectId.TryParse(id, out _))
        {
            return null;
        }

        var filter = Builders<Owner>.Filter.Eq(o => o.IdOwner, id);
        return await _context.Owners.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<List<Owner>> GetAllAsync()
    {
        return await _context.Owners
            .Find(_ => true)
            .SortBy(o => o.Name)
            .ToListAsync();
    }

    public async Task<Owner> AddAsync(Owner owner)
    {
        owner.CreatedAt = DateTime.UtcNow;
        owner.UpdatedAt = DateTime.UtcNow;
        await _context.Owners.InsertOneAsync(owner);
        return owner;
    }

    public async Task<bool> UpdateAsync(Owner owner)
    {
        if (!ObjectId.TryParse(owner.IdOwner, out _))
        {
            return false;
        }

        owner.UpdatedAt = DateTime.UtcNow;

        var filter = Builders<Owner>.Filter.Eq(o => o.IdOwner, owner.IdOwner);
        var result = await _context.Owners.ReplaceOneAsync(filter, owner);

        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        if (!ObjectId.TryParse(id, out _))
        {
            return false;
        }

        var filter = Builders<Owner>.Filter.Eq(o => o.IdOwner, id);
        var result = await _context.Owners.DeleteOneAsync(filter);

        return result.IsAcknowledged && result.DeletedCount > 0;
    }
}
