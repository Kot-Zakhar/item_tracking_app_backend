using Domain.Common.Interfaces;
using Domain.Locations.Interfaces;

namespace Domain.Locations.Interfaces;

public interface ILocationUniquenessChecker : INameUniquenessChecker<Location>;