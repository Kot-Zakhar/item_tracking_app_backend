using Application.Users.Interfaces;
using Domain.Users;
using Infrastructure.Interfaces.Common;
using Infrastructure.Interfaces.Users;

namespace Infrastructure.Services.Users;

public class UserService(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    IUserUniquenessChecker userUniquenessChecker) : IUserService
{
    public async Task<uint> CreateUserAsync(
        string firstName, string lastName, string phone, string email,
        string password, string passwordConfirmation, CancellationToken ct = default)
    {
        if (password != passwordConfirmation)
        {
            throw new ArgumentException("Password and Password confirmation do not match.");
        }

        if (!await userUniquenessChecker.IsEmailUniqueAsync(email, ct))
        {
            throw new ArgumentException("User with this email already exists.");
        }

        if (!await userUniquenessChecker.IsPhoneUniqueAsync(phone, ct))
        {
            throw new ArgumentException("User with this phone number already exists.");
        }

        var (hashedPassword, salt) = passwordHasher.HashPassword(password);

        var user = User.Create(firstName, lastName, email, phone, hashedPassword, salt);

        user = await userRepository.CreateAsync(user, ct);
        var success = await unitOfWork.SaveChangesAsync(ct);
        
        if (user.Id == 0 || !success)
        {
            throw new Exception("Failed to create user.");
        }

        return user.Id;
    }

    public async Task UpdateUserAsync(uint id, string? firstName, string? lastName, string? phone, CancellationToken ct = default)
    {
        var user = await userRepository.GetByIdAsync(id, ct);
        if (user == null) 
        {
            throw new ArgumentException("User not found.");
        }

        user.UpdateDetails(firstName, lastName, phone);

        await userRepository.UpdateAsync(user, ct);

        var success = await unitOfWork.SaveChangesAsync(ct);
        if (!success)
        {
            throw new Exception("Failed to update user.");
        }
    }

    public async Task UpdatePasswordAsync(uint id, string currentPassword, string newPassword, string newPasswordConfirmation, CancellationToken ct = default)
    {
        if (newPassword != newPasswordConfirmation)
        {
            throw new ArgumentException("New password and confirmation do not match.");
        }

        var user = await userRepository.GetByIdAsync(id, ct);
        if (user == null)
        {
            throw new ArgumentException("User not found.");
        }

        var (currentHashedPassword, currentSalt) = user.GetAuthenticationData();
        if (!passwordHasher.VerifyPassword(currentPassword, currentHashedPassword, currentSalt))
        {
            throw new ArgumentException("Current password is incorrect.");
        }

        var (hashedPassword, salt) = passwordHasher.HashPassword(newPassword);

        user.SetAuthenticationData(hashedPassword, salt);

        await userRepository.UpdateAsync(user, ct);

        var result = await unitOfWork.SaveChangesAsync(ct);
        if (!result)
        {
            throw new Exception("Failed to update user password.");
        }
    }

    public async Task DeleteUserAsync(uint id, CancellationToken ct = default)
    {
        var result = await userRepository.DeleteAsync(id, ct);
        var success = await unitOfWork.SaveChangesAsync(ct);
        
        if (!result || !success)
        {
            throw new Exception("Failed to delete user.");
        }
    }
}