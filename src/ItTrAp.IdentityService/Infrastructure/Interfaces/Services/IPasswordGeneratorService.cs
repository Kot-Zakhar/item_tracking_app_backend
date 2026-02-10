namespace ItTrAp.IdentityService.Infrastructure.Interfaces.Services;

public interface IPasswordGeneratorService
{
    string GenerateRandomPassword(int length = 8, bool includeUpperLetters = false, bool includeNumbers = false, bool includeSpecialCharacters = false);
}