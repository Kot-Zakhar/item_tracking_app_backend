namespace Domain.Common.Interfaces;

public interface INameUniquenessChecker<T>
{
    Task<bool> IsUniqueAsync(string name, CancellationToken ct = default);
}