using ItTrAp.UserService.Domain.Aggregates;

namespace ItTrAp.UserService.Infrastructure.Interfaces.Persistence.Repositories;

public interface IUserRepository : IRepository<User, uint>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
}
