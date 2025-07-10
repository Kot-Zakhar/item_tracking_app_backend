namespace Application.UserSelfManagement.DTOs;

public class UpdateUserSelfPasswordDto
{
    public required string Password { get; set; }
    public required string NewPassword { get; set; }
    public required string NewPasswordConfirmation { get; set; }
}