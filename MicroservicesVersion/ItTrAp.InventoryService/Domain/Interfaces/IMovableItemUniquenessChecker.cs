using ItTrAp.InventoryService.Domain.Models;

namespace ItTrAp.InventoryService.Domain.Interfaces;

public interface IMovableItemUniquenessChecker : INameUniquenessChecker<MovableItem, Guid> { }