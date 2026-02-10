using ItTrAp.LocationService.Domain.Aggregates;

namespace ItTrAp.LocationService.Domain.Interfaces;

public interface ILocationUniquenessChecker : INameUniquenessChecker<Location, uint>;