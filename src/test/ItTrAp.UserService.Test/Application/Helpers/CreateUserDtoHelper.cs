using ItTrAp.UserService.Application.DTOs;

namespace ItTrAp.UserService.Test.Application.Helpers;

public static class CreateUserDtoHelper
{
    public static CreateUserDto ToCreateUserDto(this UserDto user)
    {
        return new CreateUserDto
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Phone = user.Phone,
            Avatar = user.Avatar
        };
    }
}