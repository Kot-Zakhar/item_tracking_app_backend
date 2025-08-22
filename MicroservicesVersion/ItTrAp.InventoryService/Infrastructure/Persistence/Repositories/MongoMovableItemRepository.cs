using ItTrAp.InventoryService.Infrastructure.Interfaces.Persistence;
using ItTrAp.InventoryService.Infrastructure.Interfaces.Persistence.Repositories;
using ItTrAp.InventoryService.Domain.Aggregates;
using MongoDB.Driver;
using Microsoft.EntityFrameworkCore;

namespace ItTrAp.InventoryService.Infrastructure.Persistence.Repositories;

public class MongoMovableItemRepository : IRepository<MovableItem, Guid>, IMovableItemRepository
{
    private readonly IMongoCollection<MovableItem> _collection;
    private readonly AppDbContext _dbContext;

    public MongoMovableItemRepository(IMongoDatabase database, AppDbContext dbContext)
    {
        _collection = database.GetCollection<MovableItem>("movable_items");
        _dbContext = dbContext;
    }

    public async Task<MovableItem> CreateAsync(MovableItem entity, CancellationToken ct = default)
    {
        try
        {
            var savedEntity = await _dbContext.MovableItems.AddAsync(entity, ct);
            await _dbContext.SaveChangesAsync(ct);
            await _collection.InsertOneAsync(savedEntity.Entity, cancellationToken: ct);
        }
        catch (Exception)
        {
            await DeleteAsync(entity.Id, ct);
            throw;
        }
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
        var item = await _dbContext.MovableItems.FirstOrDefaultAsync(i => i.Id == id);
        if (item == null)
            return false;

        var result = await _collection.DeleteOneAsync(item => item.Id == id, ct);

        _dbContext.MovableItems.Remove(item);
        await _dbContext.SaveChangesAsync(ct);

        return result.DeletedCount > 0;
    }
}