using System.Security.Cryptography;
using System.Text;
using Abstractions;
using Abstractions.Users;

namespace Infrastructure.Services.Users;

public class PasswordHasher : IPasswordHasher
{
    private static readonly int SaltSize = 16;
    private readonly byte[] pepper;

    public PasswordHasher(IInfrastructureGlobalConfig config)
    {
        var pepperString = config.PasswordPepper;

        if (string.IsNullOrEmpty(pepperString))
        {
            throw new Exception("Password pepper is not configured.");
        }

        pepper = Encoding.ASCII.GetBytes(pepperString!);
    }


    public (byte[] hashedPassword, byte[] salt) HashPassword(string password)
    {
        var salt = GenerateSalt();
        var hashedPassword = HashPassword(password, salt, pepper);
        return (hashedPassword, salt);
    }

    public bool VerifyPassword(string password, byte[] hashedPassword, byte[] salt)
    {
        var hashedInputPassword = HashPassword(password, salt, pepper);
        return hashedInputPassword.SequenceEqual(hashedPassword);
    }

    private byte[] GenerateSalt()
    {
        using (var rng = RandomNumberGenerator.Create())
        {
            byte[] salt = new byte[SaltSize];
            rng.GetBytes(salt);
            return salt;
        }
    }

    private static byte[] HashPassword(string password, byte[] salt, byte[] pepper)
    {
        using (var hmac = new HMACSHA256(salt))
        {
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var saltedPepperedPassword = new byte[passwordBytes.Length + salt.Length + pepper.Length];

            Buffer.BlockCopy(passwordBytes, 0, saltedPepperedPassword, 0, passwordBytes.Length);
            Buffer.BlockCopy(salt, 0, saltedPepperedPassword, passwordBytes.Length, salt.Length);
            Buffer.BlockCopy(pepper, 0, saltedPepperedPassword, passwordBytes.Length + salt.Length, pepper.Length);

            return hmac.ComputeHash(saltedPepperedPassword);
        }
    }
}
