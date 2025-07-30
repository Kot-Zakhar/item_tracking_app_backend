namespace ItTrAp.ManagementService.Infrastructure.Interfaces;

public interface IUnitOfWork
{
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task AbortChangesAsync(CancellationToken ct = default);
    Task<List<TEntity>> MaterializeAsync<TEntity>(IQueryable<TEntity> query, CancellationToken ct = default);
}