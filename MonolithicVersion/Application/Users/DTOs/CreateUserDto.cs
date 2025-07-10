namespace Application.Users.DTOs;

public class CreateUserDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Phone { get; set; }
    public required string Email { get; set; }
    public string? Avatar { get; set; }
    public required string Password { get; set; }
    public required string PasswordConfirmation { get; set; }

}