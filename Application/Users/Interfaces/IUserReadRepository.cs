namespace Application.Users.Interfaces;

using Application.Common.DTOs;

public interface IUserReadRepository
{
    Task<UserDto?> GetByIdAsync(uint id, CancellationToken ct = default);
    Task<List<UserDto>> GetAllFiltered(string? search, int? top, CancellationToken ct = default);
}