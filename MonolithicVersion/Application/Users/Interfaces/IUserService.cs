using Application.Users.DTOs;

namespace Application.Users.Interfaces;

public interface IUserService
{
    Task<uint> CreateUserAsync(CreateUserDto userDto, CancellationToken ct = default);
    Task UpdateUserAsync(uint id, UpdateUserDto userDto, CancellationToken ct = default);
    Task ResetPasswordAsync(uint id, ResetUserPasswordDto credentials, CancellationToken ct = default);
    Task DeleteUserAsync(uint id, CancellationToken ct = default);
}