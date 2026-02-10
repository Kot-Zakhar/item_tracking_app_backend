using ItTrAp.UserService.Application.DTOs;

namespace ItTrAp.UserService.Application.Interfaces;

// TODO: use collection interfaces instead of concrete ones

public interface IUserReadRepository
{
    Task<UserDto?> GetByIdAsync(uint id, CancellationToken ct = default);
    Task<List<UserDto>> GetByIdsAsync(List<uint> ids, CancellationToken ct = default);
    Task<IList<UserDto>> GetAllAsync(CancellationToken ct = default);
    Task<List<UserDto>> GetAllFiltered(string? search, int? top, CancellationToken ct = default);
}