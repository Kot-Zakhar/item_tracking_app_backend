using ItTrAp.UserService.Application.Interfaces;
using ItTrAp.UserService.Application.DTOs;
using Microsoft.Extensions.Options;
using ItTrAp.UserService.Domain.Aggregates;
using ItTrAp.UserService.Domain.Interfaces;
using ItTrAp.UserService.Infrastructure.Interfaces.Persistence.Repositories;
using ItTrAp.UserService.Infrastructure.Interfaces.Persistence;
using ItTrAp.UserService.Domain.Events.OutboundEvents;
using MediatR;

namespace ItTrAp.UserService.Infrastructure.Services;

public class UserService(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IUserUniquenessChecker userUniquenessChecker,
    IOptions<GlobalConfig> config,
    IMediator mediator) : IUserService
{
    public async Task<uint> CreateUserAsync(CreateUserDto userDto, CancellationToken ct = default)
    {
        if (!await userUniquenessChecker.IsEmailUniqueAsync(userDto.Email, ct))
        {
            throw new ArgumentException("User with this email already exists.");
        }

        if (!await userUniquenessChecker.IsPhoneUniqueAsync(userDto.Phone, ct))
        {
            throw new ArgumentException("User with this phone number already exists.");
        }

        var avatar = !string.IsNullOrWhiteSpace(userDto.Avatar)
            ? userDto.Avatar
            : string.Format(config.Value.UserAvatarUrlTemplate, userDto.FirstName + "%20" + userDto.LastName);

        var user = User.Create(userDto.FirstName, userDto.LastName, userDto.Email, userDto.Phone, avatar);

        user = await userRepository.CreateAsync(user, ct);
        var success = await unitOfWork.SaveChangesAsync(ct);

        if (user.Id == 0 || !success)
        {
            throw new Exception("Failed to create user.");
        }

        await mediator.Publish(new UserCreated(user.Id, userDto.Email), ct);

        return user.Id;
    }

    public async Task UpdateUserAsync(uint id, UpdateUserDto userDto, CancellationToken ct = default)
    {
        var user = await userRepository.GetByIdAsync(id, ct);
        if (user == null)
        {
            throw new ArgumentException("User not found.");
        }


        var avatar = !string.IsNullOrWhiteSpace(userDto.Avatar)
            ? userDto.Avatar
            : string.Format(config.Value.UserAvatarUrlTemplate, userDto.FirstName + "%20" + userDto.LastName);

        user.Update(userDto.FirstName, userDto.LastName, userDto.Phone, avatar);

        await userRepository.UpdateAsync(user, ct);

        var success = await unitOfWork.SaveChangesAsync(ct);
        if (!success)
        {
            throw new Exception("Failed to update user.");
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

        await mediator.Publish(new UserDeleted(id), ct);
    }
}