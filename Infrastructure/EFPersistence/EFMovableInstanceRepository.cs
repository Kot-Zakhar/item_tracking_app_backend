using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EFPersistence;

public class EFMovableInstanceRepository(AppDbContext dbContext) : EFRepository<MovableInstance>(dbContext), IMovableInstanceRepository
{
    public Task<MovableInstance?> GetByCodeAsync(Guid code, CancellationToken ct = default)
    {
        return _dbSet.FirstOrDefaultAsync(item => item.Code == code, ct);
    }
}
