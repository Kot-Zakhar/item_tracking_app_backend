using ItTrAp.UserService.Application.DTOs;

namespace ItTrAp.UserService.Test.Application.Helpers;

public static class UserDtoHelper
{
    public static UserDto CreateValidUserDto()
    {
        return new UserDto
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Phone = "+1234567890",
            Avatar = "https://example.com/avatar.jpg"
        };
    }

    public static UserDto WithId(this UserDto user, uint id)
    {
        user.Id = id;
        return user;
    }

    public static UserDto WithFirstName(this UserDto user, string? firstName)
    {
        user.FirstName = firstName!;
        return user;
    }

    public static UserDto WithLastName(this UserDto user, string? lastName)
    {
        user.LastName = lastName!;
        return user;
    }

    public static UserDto WithEmail(this UserDto user, string? email)
    {
        user.Email = email!;
        return user;
    }

    public static UserDto WithPhone(this UserDto user, string? phone)
    {
        user.Phone = phone!;
        return user;
    }

    public static UserDto WithAvatar(this UserDto user, string? avatar)
    {
        user.Avatar = avatar;
        return user;
    }
}