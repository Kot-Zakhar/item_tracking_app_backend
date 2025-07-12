using Microsoft.EntityFrameworkCore;
using ItTrAp.InventoryService.Interfaces;
using ItTrAp.InventoryService.DTOs.MovableItems;
using ItTrAp.InventoryService.DTOs.Categories;
using ItTrAp.InventoryService.Interfaces.Repositories;

namespace ItTrAp.InventoryService.Persistence.Repositories;

// TODO: Use AutoMapper to map joined entities
// TODO: Fix Avatar property in UserViewModel
// TODO: Apply filters to the query

public class EfMovableItemReadRepository : IMovableItemReadRepository, IMovableItemUniquenessChecker
{
    private readonly AppDbContext _dbContext;

    public EfMovableItemReadRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<MovableItemDto>> GetAllFilteredAsync(
        MovableItemFiltersDto filters,
        CancellationToken ct = default
    )
    {
        var query = _dbContext.MovableItems
              .AsNoTracking();

        if (filters.CategoryIds != null && filters.CategoryIds.Count > 0)
        {
            query = query.Where(item => filters.CategoryIds.Contains(item.Category.Id));
        }

        if (!string.IsNullOrWhiteSpace(filters.Search))
        {
            var search = filters.Search.ToLower();
            query = query.Where(item => item.Name.ToLower().Contains(search));
        }

        return await query
            .AsNoTracking()
            .Select(item => new MovableItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Category = new CategoryDto
                {
                    Id = item.Category.Id,
                    Name = item.Category.Name,
                    Icon = item.Category.Icon
                },
                Visibility = item.Visibility,
                CreatedAt = item.CreatedAt,
                ImgSrc = item.ImgSrc,
            })
            .ToListAsync(ct);
    }

    public Task<MovableItemDto?> GetByIdAsync(uint id, CancellationToken ct = default)
    {
        return _dbContext.MovableItems
            .AsNoTracking()
            .Include(item => item.Category)
            .Where(item => item.Id == id)
            .Select(item => new MovableItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Category = new CategoryDto
                {
                    Id = item.Category.Id,
                    Name = item.Category.Name,
                    Icon = item.Category.Icon
                },
                Visibility = item.Visibility,
                CreatedAt = item.CreatedAt,
                ImgSrc = item.ImgSrc,
            })
            .FirstOrDefaultAsync(ct);
    }

    public Task<bool> IsUniqueAsync(string name, CancellationToken ct = default)
        => _dbContext.MovableItems.AllAsync(i => i.Name != name, ct);

    public Task<bool> IsUniqueAsync(uint id, string name, CancellationToken ct = default)
        => _dbContext.MovableItems.Where(x => x.Id != id).AllAsync(x => x.Name != name, ct);
}