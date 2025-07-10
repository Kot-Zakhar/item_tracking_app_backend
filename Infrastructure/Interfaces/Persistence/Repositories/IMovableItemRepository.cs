using Domain.Aggregates.MovableInstances;

namespace Infrastructure.Interfaces.Persistence.Repositories;

public interface IMovableInstanceRepository : IRepository<MovableInstance>
{
    Task<MovableInstance?> GetByCodeAsync(Guid code, CancellationToken ct = default);
}