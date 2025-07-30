using ItTrAp.ManagementService.Domain.Aggregates;

namespace ItTrAp.ManagementService.Infrastructure.Interfaces.Persistence.Repositories;

public interface IMovableInstanceRepository : IRepository<MovableInstance, uint>
{
    Task<MovableInstance?> GetByCodeAsync(Guid code, CancellationToken ct = default);
}