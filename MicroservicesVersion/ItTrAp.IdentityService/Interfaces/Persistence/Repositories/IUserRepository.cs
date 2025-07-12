using ItTrAp.IdentityService.Models;

namespace ItTrAp.IdentityService.Interfaces.Persistence.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
}
