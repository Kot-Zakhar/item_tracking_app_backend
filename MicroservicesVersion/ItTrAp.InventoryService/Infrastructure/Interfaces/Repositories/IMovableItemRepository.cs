using ItTrAp.InventoryService.Domain.Models;

namespace ItTrAp.InventoryService.Infrastructure.Interfaces.Repositories;

public interface IMovableItemRepository : IRepository<MovableItem, Guid>;
