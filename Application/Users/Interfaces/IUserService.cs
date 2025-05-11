namespace Application.Users.Interfaces;

public interface IUserService
{
    Task<uint> CreateUserAsync(string firstName, string lastName, string phone, string email, 
                             string password, string passwordConfirmation);
    Task UpdateUserAsync(uint id, string? firstName, string? lastName, string? phone);
    Task UpdatePasswordAsync(uint id, string currentPassword, string newPassword, string newPasswordConfirmation);
    Task DeleteUserAsync(uint id);
}