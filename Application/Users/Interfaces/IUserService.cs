namespace Application.Users.Interfaces;

public interface IUserService
{
    Task<uint> CreateUserAsync(string firstName, string lastName, string phone, string email, 
                             string password, string passwordConfirmation, CancellationToken ct = default);
    Task UpdateUserAsync(uint id, string? firstName, string? lastName, string? phone, CancellationToken ct = default);
    Task UpdatePasswordAsync(uint id, string currentPassword, string newPassword, string newPasswordConfirmation, CancellationToken ct = default);
    Task DeleteUserAsync(uint id, CancellationToken ct = default);
}