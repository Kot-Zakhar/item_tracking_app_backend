using ItTrAp.ManagementService.Domain.Aggregates;

namespace ItTrAp.ManagementService.Infrastructure.Interfaces.Persistence.Repositories;

public interface ILocationRepository : IRepository<Location, uint>
{
    Task<Location?> GetByCodeAsync(Guid code, CancellationToken ct = default);
}