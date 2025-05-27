
using Domain.MovableItems;
using Infrastructure.Interfaces.MovableInstances;
using Infrastructure.Persistence.Common;

namespace Infrastructure.Persistence.MovableInstances;

public class EFMovableInstanceRepository(AppDbContext dbContext) : EFRepository<MovableInstance>(dbContext), IMovableInstanceRepository;
