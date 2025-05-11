using Application.Common.Interfaces;
using Application.Users.Interfaces;
using Domain.Users;

namespace Infrastructure.Services.Users;

public class UserService(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    IUserUniquenessChecker userUniquenessChecker) : IUserService
{
    public async Task<uint> CreateUserAsync(
        string firstName, string lastName, string phone, string email,
        string password, string passwordConfirmation)
    {
        if (password != passwordConfirmation)
        {
            throw new ArgumentException("Password and Password confirmation do not match.");
        }

        if (!await userUniquenessChecker.IsEmailUniqueAsync(email))
        {
            throw new ArgumentException("User with this email already exists.");
        }

        if (!await userUniquenessChecker.IsPhoneUniqueAsync(phone))
        {
            throw new ArgumentException("User with this phone number already exists.");
        }

        var (hashedPassword, salt) = passwordHasher.HashPassword(password);

        var user = User.Create(firstName, lastName, email, phone, hashedPassword, salt);

        user = await userRepository.CreateAsync(user);
        var success = await unitOfWork.SaveChangesAsync();
        
        if (user.Id == 0 || !success)
        {
            throw new Exception("Failed to create user.");
        }

        return user.Id;
    }

    public async Task UpdateUserAsync(uint id, string? firstName, string? lastName, string? phone)
    {
        var user = await userRepository.GetByIdAsync(id);
        if (user == null) 
        {
            throw new ArgumentException("User not found.");
        }

        user.UpdateDetails(firstName, lastName, phone);

        await userRepository.UpdateAsync(user);

        var success = await unitOfWork.SaveChangesAsync();
        if (!success)
        {
            throw new Exception("Failed to update user.");
        }
    }

    public async Task UpdatePasswordAsync(uint id, string currentPassword, string newPassword, string newPasswordConfirmation)
    {
        if (newPassword != newPasswordConfirmation)
        {
            throw new ArgumentException("New password and confirmation do not match.");
        }

        var user = await userRepository.GetByIdAsync(id);
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

        await userRepository.UpdateAsync(user);

        var result = await unitOfWork.SaveChangesAsync();
        if (!result)
        {
            throw new Exception("Failed to update user password.");
        }
    }

    public async Task DeleteUserAsync(uint id)
    {
        var result = await userRepository.DeleteAsync(id);
        var success = await unitOfWork.SaveChangesAsync();
        
        if (!result || !success)
        {
            throw new Exception("Failed to delete user.");
        }
    }
}