using ItTrAp.InventoryService.Application.DTOs.MovableInstances;
using ItTrAp.InventoryService.Application.Interfaces.Repositories;
using ItTrAp.InventoryService.Domain.Aggregates;
using ItTrAp.InventoryService.Infrastructure.Interfaces.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ItTrAp.InventoryService.Infrastructure.Persistence.Repositories;

public class EFMovableInstanceRepository(AppDbContext dbContext) : EFRepository<MovableInstance, uint>(dbContext), IMovableInstanceRepository, IMovableInstanceReadRepository
{
    protected AppDbContext dbContext = dbContext;
    
    public Task<List<MovableInstanceDto>> GetAllAsync(Guid itemId, CancellationToken ct = default)
    {
        return dbContext.MovableInstances
            .AsNoTracking()
            .Include(instance => instance.MovableItem)
            .Where(instance => instance.MovableItem.Id == itemId)
            .Select(instance => new MovableInstanceDto
            {
                Id = instance.Id,
                MovableItemId = instance.MovableItem.Id,
                CreatedAt = instance.CreatedAt,
            })
            .ToListAsync(ct);
    }

    public Task<MovableInstanceDto?> GetByIdAsync(Guid itemId, uint id, CancellationToken ct = default)
    {
        return dbContext.MovableInstances
            .AsNoTracking()
            .Include(instance => instance.MovableItem)
            .Where(instance => instance.MovableItem.Id == itemId && instance.Id == id)
            .Select(instance => new MovableInstanceDto
            {
                Id = instance.Id,
                MovableItemId = instance.MovableItem.Id,
                CreatedAt = instance.CreatedAt,
            })
            .FirstOrDefaultAsync(ct);
    }

    public async Task<IList<MovableInstanceDto>> GetByItemIdAsync(Guid itemId, CancellationToken ct = default)
    {
        return await dbContext.MovableInstances
            .AsNoTracking()
            .Include(instance => instance.MovableItem)
            .Where(instance => instance.MovableItem.Id == itemId)
            .Select(instance => new MovableInstanceDto
            {
                Id = instance.Id,
                MovableItemId = instance.MovableItem.Id,
                CreatedAt = instance.CreatedAt,
            })
            .ToListAsync(ct);
    }

    public async Task<IList<int>> GetInstanceAmountsByItemIdsAsync(IList<Guid> itemIds, CancellationToken ct = default)
    {
        var counts = await dbContext.MovableInstances
            .AsNoTracking()
            .Where(instance => itemIds.Contains(instance.MovableItem.Id))
            .GroupBy(instance => instance.MovableItem.Id)
            .ToDictionaryAsync(group => group.Key, group => group.Count(), ct);

        return itemIds.Select(id => counts.ContainsKey(id) ? counts[id] : 0).ToList();
    }
}
