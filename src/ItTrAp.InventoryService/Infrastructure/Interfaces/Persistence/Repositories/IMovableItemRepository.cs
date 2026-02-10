using ItTrAp.InventoryService.Domain.Aggregates;

namespace ItTrAp.InventoryService.Infrastructure.Interfaces.Persistence.Repositories;

public interface IMovableItemRepository : IRepository<MovableItem, Guid>;
