using ItTrAp.UserService.Application.DTOs;

namespace ItTrAp.UserService.Test.Application.Helpers;

public static class UpdateUserDtoHelper
{
    public static UpdateUserDto ToUpdateUserDto(this UserDto userDto)
    {
        return new UpdateUserDto
        {
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            Phone = userDto.Phone,
            Avatar = userDto.Avatar
        };
    }
}