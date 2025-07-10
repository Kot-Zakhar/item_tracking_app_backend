using Domain.Aggregates.MovableInstances;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Interfaces.Persistence.Repositories;

namespace Infrastructure.EFPersistence.Repositories;

public class EFMovableInstanceRepository(AppDbContext dbContext) : EFRepository<MovableInstance>(dbContext), IMovableInstanceRepository
{
    public Task<MovableInstance?> GetByCodeAsync(Guid code, CancellationToken ct = default)
    {
        return _dbSet.FirstOrDefaultAsync(item => item.Code == code, ct);
    }
}
