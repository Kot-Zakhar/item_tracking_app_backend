using Application.Users.DTOs;

namespace Application.Users.Interfaces;

public interface IUserService
{
    Task<uint> CreateUserAsync(CreateUserDto user, CancellationToken ct = default);
    Task UpdateUserAsync(uint id, UpdateUserDto user, CancellationToken ct = default);
    Task UpdatePasswordAsync(uint id, UpdatePasswordDto credentials, CancellationToken ct = default);
    Task DeleteUserAsync(uint id, CancellationToken ct = default);
}