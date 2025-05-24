using Domain.MovableItems;
using Infrastructure.Interfaces.MovableItems;
using Infrastructure.Persistence.Common;

namespace Infrastructure.Persistence.MovableItems;

public class EFMovableItemRepository(AppDbContext dbContext) : EFRepository<MovableItem>(dbContext), IMovableItemRepository;
