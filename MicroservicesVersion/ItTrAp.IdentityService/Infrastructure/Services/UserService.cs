using ItTrAp.IdentityService.Application.Interfaces.Services;
using ItTrAp.IdentityService.Application.Users.DTOs;
using ItTrAp.IdentityService.Domain.Aggregates;
using ItTrAp.IdentityService.Infrastructure.Interfaces.Persistence;
using ItTrAp.IdentityService.Infrastructure.Interfaces.Persistence.Repositories;

namespace ItTrAp.IdentityService.Infrastructure.Services;

public class UserService(ILogger<UserService> logger, IUnitOfWork unitOfWork,IUserRepository userRepository, IPasswordHasherService passwordHasher) : IUserService
{
    public async Task CreateUserAsync(uint userId, string email, CancellationToken cancellationToken)
    {
        var existingUser = userRepository.GetAllAsync(cancellationToken)
            .Where(u => u.Email == email || u.Id == userId)
            .FirstOrDefault();

        if (existingUser != null)
        {
            return;
        }

        var randomPassword = "qwer1234"; // TODO: Think of how to generate a random password

        var (hashedPassword, salt) = passwordHasher.HashPassword(randomPassword);

        var user = User.Create(userId, email, hashedPassword, salt);
        await userRepository.CreateAsync(user, cancellationToken);
        var result = await unitOfWork.SaveChangesAsync(cancellationToken);
        if (!result)
        {
            throw new Exception("Failed to create user.");
        }

        logger.LogInformation("User created with ID: {UserId}, Email: {Email}", userId, email);
    }

    public async Task DeleteUserAsync(uint userId, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (user == null)
        {
            return;
        }
        await userRepository.DeleteAsync(userId, cancellationToken);
        var result = await unitOfWork.SaveChangesAsync(cancellationToken);
        if (!result)
        {
            throw new Exception("Failed to delete user.");
        }

        logger.LogInformation("User deleted with ID: {UserId}", userId);
    }

    public async Task ResetPasswordAsync(uint id, string newPassword, CancellationToken ct = default)
    {

        var user = await userRepository.GetByIdAsync(id, ct);
        if (user == null)
        {
            throw new ArgumentException("User not found.");
        }

        var (hashedPassword, salt) = passwordHasher.HashPassword(newPassword);

        user.SetAuthenticationData(hashedPassword, salt);

        await userRepository.UpdateAsync(user, ct);

        var result = await unitOfWork.SaveChangesAsync(ct);
        if (!result)
        {
            throw new Exception("Failed to reset user password.");
        }

        logger.LogInformation("User password reset with ID: {UserId}", id);
    }

}