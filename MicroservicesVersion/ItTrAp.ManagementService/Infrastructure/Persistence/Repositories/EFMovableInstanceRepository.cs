using ItTrAp.ManagementService.Application.DTOs.MovableInstances;
using ItTrAp.ManagementService.Application.Interfaces.Repositories;
using ItTrAp.ManagementService.Domain.Aggregates;
using ItTrAp.ManagementService.Infrastructure.Interfaces.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ItTrAp.ManagementService.Infrastructure.Persistence.Repositories;

public class EFMovableInstanceRepository(AppDbContext dbContext) : EFRepository<MovableInstance, uint>(dbContext), IMovableInstanceRepository, IMovableInstanceReadRepository
{
    protected AppDbContext dbContext = dbContext;

    public Task<MovableInstance?> GetByCodeAsync(Guid code, CancellationToken ct = default)
    {
        return _dbSet.FirstOrDefaultAsync(item => item.Code == code, ct);
    }
    
    public Task<List<MovableInstanceDto>> GetAllFilteredAsync(Guid itemId, MovableInstanceFiltersDto filters, CancellationToken ct = default)
    {
        var query = dbContext.MovableInstances
            .AsNoTracking()
            .Include(instance => instance.MovableItem)
            .Include(instance => instance.Location)
            .Include(instance => instance.User)
            .Where(instance => instance.MovableItem.Id == itemId);

        if (filters.Status.HasValue)
        {
            query = query.Where(instance => instance.Status == filters.Status.Value);
        }

        if (filters.LocationId.HasValue)
        {
            query = query.Where(instance => instance.Location != null && instance.Location.Id == filters.LocationId.Value);
        }

        if (filters.UserIds != null && filters.UserIds.Count > 0)
        {
            query = query.Where(instance => instance.User != null && filters.UserIds.Contains(instance.User.Id));
        }

        return query
            .Select(instance => new MovableInstanceDto
            {
                Id = instance.Id,
                MovableItemId = instance.MovableItem.Id,
                Code = instance.Code,
                Status = instance.Status,
                CreatedAt = instance.CreatedAt,
            })
            .ToListAsync(ct);
    }

    public Task<MovableInstanceDto?> GetByIdAsync(Guid itemId, uint id, CancellationToken ct = default)
    {
        return dbContext.MovableInstances
            .AsNoTracking()
            .Include(instance => instance.MovableItem)
            .Include(instance => instance.Location)
            .Include(instance => instance.User)
            .Where(instance => instance.MovableItem.Id == itemId && instance.Id == id)
            .Select(instance => new MovableInstanceDto
            {
                Id = instance.Id,
                MovableItemId = instance.MovableItem.Id,
                Code = instance.Code,
                Status = instance.Status,
                CreatedAt = instance.CreatedAt,
            })
            .FirstOrDefaultAsync(ct);
    }
}
