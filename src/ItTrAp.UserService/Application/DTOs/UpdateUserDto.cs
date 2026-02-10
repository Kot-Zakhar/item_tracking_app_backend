namespace ItTrAp.UserService.Application.DTOs;

public class UpdateUserDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Phone { get; set; }
    public string? Avatar { get; set; }
}