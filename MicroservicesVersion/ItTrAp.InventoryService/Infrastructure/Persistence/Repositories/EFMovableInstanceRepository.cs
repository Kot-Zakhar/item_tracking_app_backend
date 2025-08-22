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
}
