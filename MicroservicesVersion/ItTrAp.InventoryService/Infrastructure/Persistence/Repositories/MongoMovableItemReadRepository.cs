using ItTrAp.InventoryService.Application.DTOs.MovableItems;
using ItTrAp.InventoryService.Infrastructure.Interfaces.Persistence.Repositories;
using ItTrAp.InventoryService.Domain.Aggregates;
using ItTrAp.InventoryService.Domain.Interfaces;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace ItTrAp.InventoryService.Infrastructure.Persistence.Repositories;

public class MongoMovableItemReadRepository : IMovableItemReadRepository, IMovableItemUniquenessChecker
{
    private readonly IMongoCollection<MovableItem> _collection;

    public MongoMovableItemReadRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<MovableItem>("movable_items");
    }

    public async Task<List<MovableItemDto>> GetAllFilteredAsync(
        MovableItemFiltersDto filters,
        CancellationToken ct = default
    )
    {
        var query = _collection.AsQueryable();

        if (filters.Ids != null && filters.Ids.Count > 0)
        {
            query = query.Where(item => filters.Ids.Contains(item.Id));
        }

        if (filters.CategoryIds != null && filters.CategoryIds.Count > 0)
        {
            query = query.Where(item => filters.CategoryIds.Contains(item.CategoryId));
        }

        if (!string.IsNullOrWhiteSpace(filters.Search))
        {
            var search = filters.Search.ToLower();
            query = query.Where(item => item.Name.ToLower().Contains(search));
        }

        return await query
            .Select(item => new MovableItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                CategoryId = item.CategoryId,
                CreatedAt = item.CreatedAt,
                ImgSrc = item.ImgSrc,
            })
            .ToListAsync(ct);
    }

    public async Task<List<MovableItemDto>> GetAllAsync(CancellationToken ct = default)
    {
        return await _collection.AsQueryable()
            .Select(item => new MovableItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                CategoryId = item.CategoryId,
                CreatedAt = item.CreatedAt,
                ImgSrc = item.ImgSrc,
            })
            .ToListAsync(ct);
    }

    public async Task<MovableItemDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var item = await _collection.Find(i => i.Id == id).FirstOrDefaultAsync(ct);
        if (item == null) return null;

        return new MovableItemDto
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            CategoryId = item.CategoryId,
            CreatedAt = item.CreatedAt,
            ImgSrc = item.ImgSrc,
        };
    }

    public async Task<bool> IsUniqueAsync(string name, CancellationToken ct = default)
    {
        return !await _collection.Find(item => item.Name == name).AnyAsync(ct);
    }

    public async Task<bool> IsUniqueAsync(Guid id, string name, CancellationToken ct = default)
    {
        return !await _collection.Find(item => item.Id != id && item.Name == name).AnyAsync(ct);
    }
}