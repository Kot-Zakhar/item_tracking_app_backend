using Domain.Aggregates.Locations;

namespace Domain.Interfaces;

public interface ILocationUniquenessChecker : INameUniquenessChecker<Location>;