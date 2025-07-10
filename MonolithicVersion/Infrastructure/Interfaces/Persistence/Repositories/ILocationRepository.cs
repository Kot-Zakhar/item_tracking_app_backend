using Domain.Aggregates.Locations;

namespace Infrastructure.Interfaces.Persistence.Repositories;

public interface ILocationRepository : IRepository<Location>
{
    Task<Location?> GetByCodeAsync(Guid code, CancellationToken ct = default);
}