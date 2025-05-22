using Domain.Users;
using Infrastructure.Interfaces.Common;

namespace Infrastructure.Interfaces.Users;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
}
