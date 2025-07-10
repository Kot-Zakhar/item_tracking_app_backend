namespace Application.UserSelfManagement.Interfaces;

using Application.Common.DTOs;

public interface IUserSelfManagementReadRepository
{
    Task<UserDto?> GetByIdAsync(uint id, CancellationToken ct = default);
}