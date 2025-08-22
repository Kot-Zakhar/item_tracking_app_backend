using ItTrAp.InventoryService.Infrastructure.Interfaces.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ItTrAp.InventoryService.Infrastructure.Persistence;

public class EfUnitOfWork(AppDbContext dbContext) : IUnitOfWork
{
    public async Task<bool> SaveChangesAsync(CancellationToken ct = default)
    {
        return await dbContext.SaveChangesAsync(ct) > 0;
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