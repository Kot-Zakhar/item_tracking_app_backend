using ItTrAp.LocationService.Domain.Aggregates;

namespace ItTrAp.LocationService.Infrastructure.Persistence.Interfaces.Repositories;

public interface ILocationRepository : IRepository<Location, uint>
{
    Task<Location?> GetByCodeAsync(Guid code, CancellationToken ct = default);
}