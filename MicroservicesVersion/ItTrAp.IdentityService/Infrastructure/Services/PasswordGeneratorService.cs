using ItTrAp.IdentityService.Infrastructure.Interfaces.Services;

namespace ItTrAp.IdentityService.Infrastructure.Services;

public class PasswordGeneratorService : IPasswordGeneratorService
{
    public string GenerateRandomPassword(int length = 8, bool includeUpperLetters = false, bool includeNumbers = false, bool includeSpecialCharacters = false)
    {
        const string lowerCase = "abcdefghijklmnopqrstuvwxyz";
        const string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string numbers = "0123456789";
        const string specialCharacters = "!@#$%^&*()_+[]{}|;:,.<>?";

        var characterSet = lowerCase;

        if (includeUpperLetters)
            characterSet += upperCase;
        if (includeNumbers)
            characterSet += numbers;
        if (includeSpecialCharacters)
            characterSet += specialCharacters;

        var random = new Random();
        var password = new char[length];

        for (int i = 0; i < length; i++)
        {
            password[i] = characterSet[random.Next(characterSet.Length)];
        }

        return new string(password);
    }
}