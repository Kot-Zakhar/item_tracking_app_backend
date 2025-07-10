namespace Domain.Interfaces;

public interface IUserUniquenessChecker
{
    Task<bool> IsEmailUniqueAsync(string email, CancellationToken ct = default);
    Task<bool> IsPhoneUniqueAsync(string phone, CancellationToken ct = default);
}