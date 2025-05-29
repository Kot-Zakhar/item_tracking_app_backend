namespace Application.Users.DTOs;

public class UpdatePasswordDto
{
    public required string Password { get; set; }
    public required string NewPassword { get; set; }
    public required string NewPasswordConfirmation { get; set; }
}