using Infrastructure.Interfaces.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Common;

public class EFRepository<TEntity>(AppDbContext dbContext) : IRepository<TEntity>
    where TEntity : class
{
    protected readonly DbSet<TEntity> _dbSet = dbContext.Set<TEntity>();

    public async Task<TEntity> CreateAsync(TEntity entity, CancellationToken ct = default)
    {
        var entry = await _dbSet.AddAsync(entity, ct);
        return entry.Entity;
    }

    public async Task<bool> DeleteAsync(uint id, CancellationToken ct = default)
    {
        var entity = await _dbSet.FindAsync([id], ct);
        if (entity == null)
        {
            return false;
        }

        _dbSet.Remove(entity);

        return true;
    }

    public async Task<List<TEntity>> GetAllAsync(CancellationToken ct = default)
    {
        return await _dbSet.ToListAsync(ct);
    }

    public async Task<TEntity?> GetByIdAsync(uint id, CancellationToken ct = default)
    {
        return await _dbSet.FindAsync([id], ct);
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken ct = default)
    {
        _dbSet.Update(entity);
        await Task.CompletedTask;
    }
}