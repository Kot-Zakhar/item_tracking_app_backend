using ItTrAp.InventoryService.Domain.Aggregates;

namespace ItTrAp.InventoryService.Domain.Interfaces;

public interface ICategoryUniquenessChecker : INameUniquenessChecker<Category, uint>;