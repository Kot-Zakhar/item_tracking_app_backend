using ItTrAp.IdentityService.Domain;

namespace ItTrAp.IdentityService.Infrastructure.Interfaces.Persistence.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
}
