namespace Application.Users.Interfaces;

using Application.Common.DTOs;
using Application.Users.DTOs;

public interface IUserReadRepository
{
    Task<UserDto?> GetByIdAsync(uint id, CancellationToken ct = default);
    Task<List<UserWithDetailsDto>> GetAllFiltered(string? search, int? top, CancellationToken ct = default);
}