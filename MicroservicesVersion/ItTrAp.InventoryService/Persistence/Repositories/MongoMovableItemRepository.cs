using ItTrAp.InventoryService.Interfaces;
using ItTrAp.InventoryService.Interfaces.Repositories;
using ItTrAp.InventoryService.Models;
using MongoDB.Driver;

namespace ItTrAp.InventoryService.Persistence.Repositories;

public class MongoMovableItemRepository : IRepository<MovableItem, Guid>, IMovableItemRepository
{
    private readonly IMongoCollection<MovableItem> _collection;

    public MongoMovableItemRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<MovableItem>("movable_items");
    }

    public async Task<MovableItem> CreateAsync(MovableItem entity, CancellationToken ct = default)
    {
        await _collection.InsertOneAsync(entity, cancellationToken: ct);
        return entity;
    }

    public async Task<MovableItem?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _collection.Find(item => item.Id == id).FirstOrDefaultAsync(ct);
    }

    public IQueryable<MovableItem> GetAllAsync(CancellationToken ct = default)
    {
        return _collection.AsQueryable();
    }

    public Task UpdateAsync(MovableItem entity, CancellationToken ct = default)
    {
        return _collection.ReplaceOneAsync(item => item.Id == entity.Id, entity, cancellationToken: ct);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var result = await _collection.DeleteOneAsync(item => item.Id == id, ct);
        return result.DeletedCount > 0;
    }
}