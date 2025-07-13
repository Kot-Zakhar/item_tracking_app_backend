using ItTrAp.InventoryService.Models;

namespace ItTrAp.InventoryService.Interfaces;

public interface IMovableItemUniquenessChecker : INameUniquenessChecker<MovableItem, Guid> { }