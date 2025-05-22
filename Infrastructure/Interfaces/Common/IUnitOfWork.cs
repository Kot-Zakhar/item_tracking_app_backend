namespace Infrastructure.Interfaces.Common;

public interface IUnitOfWork
{
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task AbortChangesAsync();
}