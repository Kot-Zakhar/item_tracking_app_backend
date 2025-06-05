using Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EFPersistence;

public class EfUnitOfWork(AppDbContext dbContext) : IUnitOfWork
{
    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public Task AbortChangesAsync(CancellationToken ct = default)
    {
        dbContext.ChangeTracker.Clear();
        return Task.CompletedTask;
    }

    public async Task<List<TEntity>> MaterializeAsync<TEntity>(IQueryable<TEntity> query, CancellationToken ct = default)
    {
        return await query.ToListAsync(ct);
    }
}