using ItTrAp.IdentityService.Application.Users.DTOs;

namespace ItTrAp.IdentityService.Application.Interfaces.Services;

public interface IUserService
{
    Task CreateUserAsync(uint userId, string email, CancellationToken cancellationToken);
    Task DeleteUserAsync(uint userId, CancellationToken cancellationToken);
    Task ResetPasswordAsync(uint userId, string newPassword, CancellationToken ct = default);
}