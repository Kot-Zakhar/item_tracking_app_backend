using Application.Common.DTOs;

namespace Application.Users.DTOs;

public class UserWithDetailsDto : UserDto
{
    public uint ItemsAmount { get; set; }
}