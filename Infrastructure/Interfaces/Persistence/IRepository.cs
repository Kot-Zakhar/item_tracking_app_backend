namespace Infrastructure.Interfaces.Persistence;

public interface IRepository<TEntity>
    where TEntity : class
{
    Task<TEntity> CreateAsync(TEntity entity, CancellationToken ct = default);
    Task<TEntity?> GetByIdAsync(uint id, CancellationToken ct = default);
    IQueryable<TEntity> GetAllAsync(CancellationToken ct = default);
    Task UpdateAsync(TEntity entity, CancellationToken ct = default);
    Task<bool> DeleteAsync(uint id, CancellationToken ct = default);
}