namespace ItTrAp.ManagementService.Domain.Interfaces;

public interface INameUniquenessChecker<T, TId>
{
    Task<bool> IsUniqueAsync(string name, CancellationToken ct = default);

    Task<bool> IsUniqueAsync(TId id, string name, CancellationToken ct = default);
}