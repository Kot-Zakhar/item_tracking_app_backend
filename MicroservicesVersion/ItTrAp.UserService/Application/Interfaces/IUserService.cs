using ItTrAp.UserService.Application.DTOs;

namespace ItTrAp.UserService.Application.Interfaces;

public interface IUserService
{
    Task<uint> CreateUserAsync(CreateUserDto userDto, CancellationToken ct = default);
    Task UpdateUserAsync(uint id, UpdateUserDto userDto, CancellationToken ct = default);
    Task DeleteUserAsync(uint id, CancellationToken ct = default);
}