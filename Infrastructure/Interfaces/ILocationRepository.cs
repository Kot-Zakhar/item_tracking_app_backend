using Abstractions;
using Domain.Locations;

namespace Infrastructure.Interfaces;

public interface ILocationRepository : IRepository<Location>
{
    Task<Location?> GetByCodeAsync(Guid code, CancellationToken ct = default);
}