namespace Abstractions.Users;

public interface IPasswordHasher
{
    (byte[] hashedPassword, byte[] salt) HashPassword(string password);
    bool VerifyPassword(string password, byte[] hashedPassword, byte[] salt);
}