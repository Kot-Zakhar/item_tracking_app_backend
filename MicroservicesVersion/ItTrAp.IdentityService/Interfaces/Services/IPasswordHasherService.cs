namespace ItTrAp.IdentityService.Interfaces.Services;

public interface IPasswordHasherService
{
    (byte[] hashedPassword, byte[] salt) HashPassword(string password);
    bool VerifyPassword(string password, byte[] hashedPassword, byte[] salt);
}