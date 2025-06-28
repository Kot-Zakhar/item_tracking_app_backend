namespace Application.Users.DTOs;

public class ResetUserPasswordDto
{
    public required string NewPassword { get; set; }
    public required string NewPasswordConfirmation { get; set; }
}