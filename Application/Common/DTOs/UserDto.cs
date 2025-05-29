namespace Application.Common.DTOs;

public class UserDto
{
    public uint Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Phone { get; set; }
    public required string Email { get; set; }
    public string? Avatar { get; set; }
}