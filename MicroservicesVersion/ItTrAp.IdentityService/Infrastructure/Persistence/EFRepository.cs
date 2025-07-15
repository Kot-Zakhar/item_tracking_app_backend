using ItTrAp.IdentityService.Infrastructure.Interfaces.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ItTrAp.IdentityService.Infrastructure.Persistence;

public abstract class EFRepository<TEntity>(AppDbContext dbContext) : IRepository<TEntity>
    where TEntity : class
{
    protected readonly DbSet<TEntity> _dbSet = dbContext.Set<TEntity>();

    public async Task<TEntity> CreateAsync(TEntity entity, CancellationToken ct = default)
        => (await _dbSet.AddAsync(entity, ct)).Entity;

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

    public IQueryable<TEntity> GetAllAsync(CancellationToken ct = default)
        => _dbSet;

    public async Task<TEntity?> GetByIdAsync(uint id, CancellationToken ct = default)
        => await _dbSet.FindAsync([id], ct);

    public Task UpdateAsync(TEntity entity, CancellationToken ct = default)
        => Task.FromResult(_dbSet.Update(entity));
}