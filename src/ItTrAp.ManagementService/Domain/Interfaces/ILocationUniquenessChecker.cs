using ItTrAp.ManagementService.Domain.Aggregates;

namespace ItTrAp.ManagementService.Domain.Interfaces;

public interface ILocationUniquenessChecker : INameUniquenessChecker<Location, uint>;