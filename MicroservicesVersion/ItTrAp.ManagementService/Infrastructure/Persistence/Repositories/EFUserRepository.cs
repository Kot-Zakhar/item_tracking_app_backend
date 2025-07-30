using ItTrAp.ManagementService.Domain.Aggregates;
using ItTrAp.ManagementService.Infrastructure.Interfaces.Persistence.Repositories;

namespace ItTrAp.ManagementService.Infrastructure.Persistence.Repositories;

public class EFUserRepository(AppDbContext dbContext) : EFRepository<User, uint>(dbContext), IUserRepository;