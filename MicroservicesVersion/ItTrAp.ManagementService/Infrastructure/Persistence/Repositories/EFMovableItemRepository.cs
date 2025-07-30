using ItTrAp.ManagementService.Domain.Aggregates;
using ItTrAp.ManagementService.Infrastructure.Interfaces.Persistence.Repositories;

namespace ItTrAp.ManagementService.Infrastructure.Persistence.Repositories;

public class EFMovableItemRepository(AppDbContext dbContext) : EFRepository<MovableItem, Guid>(dbContext), IMovableItemRepository;