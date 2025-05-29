using Application.Users.Interfaces;
using Domain.Users;
using Domain.Users.Interfaces;
using Abstractions.Users;
using Abstractions;
using Application.Users.DTOs;

namespace Infrastructure.Services.Users;

public class UserService(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    IUserUniquenessChecker userUniquenessChecker) : IUserService
{
    public async Task<uint> CreateUserAsync(CreateUserDto userDto, CancellationToken ct = default)
    {
        if (userDto.Password != userDto.PasswordConfirmation)
        {
            throw new ArgumentException("Password and Password confirmation do not match.");
        }

        if (!await userUniquenessChecker.IsEmailUniqueAsync(userDto.Email, ct))
        {
            throw new ArgumentException("User with this email already exists.");
        }

        if (!await userUniquenessChecker.IsPhoneUniqueAsync(userDto.Phone, ct))
        {
            throw new ArgumentException("User with this phone number already exists.");
        }

        var (hashedPassword, salt) = passwordHasher.HashPassword(userDto.Password);

        var user = User.Create(userDto.FirstName, userDto.LastName, userDto.Email, userDto.Phone, hashedPassword, salt);

        user = await userRepository.CreateAsync(user, ct);
        var success = await unitOfWork.SaveChangesAsync(ct);
        
        if (user.Id == 0 || !success)
        {
            throw new Exception("Failed to create user.");
        }

        return user.Id;
    }

    public async Task UpdateUserAsync(uint id, UpdateUserDto userDto, CancellationToken ct = default)
    {
        var user = await userRepository.GetByIdAsync(id, ct);
        if (user == null) 
        {
            throw new ArgumentException("User not found.");
        }

        user.UpdateDetails(userDto.FirstName, userDto.LastName, userDto.Phone);

        await userRepository.UpdateAsync(user, ct);

        var success = await unitOfWork.SaveChangesAsync(ct);
        if (!success)
        {
            throw new Exception("Failed to update user.");
        }
    }

    public async Task UpdatePasswordAsync(uint id, UpdatePasswordDto passwords, CancellationToken ct = default)
    {
        if (passwords.NewPassword != passwords.NewPasswordConfirmation)
        {
            throw new ArgumentException("New password and confirmation do not match.");
        }

        var user = await userRepository.GetByIdAsync(id, ct);
        if (user == null)
        {
            throw new ArgumentException("User not found.");
        }

        var (currentHashedPassword, currentSalt) = user.GetAuthenticationData();
        if (!passwordHasher.VerifyPassword(passwords.Password, currentHashedPassword, currentSalt))
        {
            throw new ArgumentException("Current password is incorrect.");
        }

        var (hashedPassword, salt) = passwordHasher.HashPassword(passwords.NewPassword);

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