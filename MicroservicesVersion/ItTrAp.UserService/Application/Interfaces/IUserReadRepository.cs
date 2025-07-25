using ItTrAp.UserService.Application.DTOs;

namespace ItTrAp.UserService.Application.Interfaces;

public interface IUserReadRepository
{
    Task<UserDto?> GetByIdAsync(uint id, CancellationToken ct = default);
    Task<List<UserDto>> GetAllFiltered(string? search, int? top, CancellationToken ct = default);
}