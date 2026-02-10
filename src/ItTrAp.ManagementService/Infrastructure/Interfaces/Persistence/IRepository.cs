namespace ItTrAp.ManagementService.Infrastructure.Interfaces;

public interface IRepository<TEntity, TId>
    where TEntity : class
{
    Task<TEntity> CreateAsync(TEntity entity, CancellationToken ct = default);
    Task<TEntity?> GetByIdAsync(TId id, CancellationToken ct = default);
    IQueryable<TEntity> GetAllAsync(CancellationToken ct = default);
    Task UpdateAsync(TEntity entity, CancellationToken ct = default);
    Task<bool> DeleteAsync(TId id, CancellationToken ct = default);
}