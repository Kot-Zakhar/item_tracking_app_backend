using Application.UserSelfManagement.DTOs;

namespace Application.UserSelfManagement.Interfaces;

public interface IUserSelfManagementService
{
    Task UpdateUserSelfAsync(uint id, UpdateUserSelfDto user, CancellationToken ct = default);
    Task UpdateUserSelfPasswordAsync(uint id, UpdateUserSelfPasswordDto credentials, CancellationToken ct = default);
}