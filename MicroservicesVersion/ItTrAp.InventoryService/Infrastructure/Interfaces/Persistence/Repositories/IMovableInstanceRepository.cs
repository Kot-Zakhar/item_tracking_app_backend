using ItTrAp.InventoryService.Domain.Aggregates;

namespace ItTrAp.InventoryService.Infrastructure.Interfaces.Persistence.Repositories;

public interface IMovableInstanceRepository : IRepository<MovableInstance, uint>;