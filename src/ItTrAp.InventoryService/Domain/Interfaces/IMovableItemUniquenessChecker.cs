using ItTrAp.InventoryService.Domain.Aggregates;

namespace ItTrAp.InventoryService.Domain.Interfaces;

public interface IMovableItemUniquenessChecker : INameUniquenessChecker<MovableItem, Guid> { }