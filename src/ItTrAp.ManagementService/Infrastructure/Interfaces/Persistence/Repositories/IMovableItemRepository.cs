using ItTrAp.ManagementService.Domain.Aggregates;

namespace ItTrAp.ManagementService.Infrastructure.Interfaces.Persistence.Repositories;

public interface IMovableItemRepository : IRepository<MovableItem, Guid>;