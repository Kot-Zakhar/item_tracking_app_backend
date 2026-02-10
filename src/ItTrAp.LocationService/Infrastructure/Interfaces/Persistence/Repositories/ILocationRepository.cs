using ItTrAp.LocationService.Domain.Aggregates;

namespace ItTrAp.LocationService.Infrastructure.Persistence.Interfaces.Repositories;

public interface ILocationRepository : IRepository<Location, uint>;