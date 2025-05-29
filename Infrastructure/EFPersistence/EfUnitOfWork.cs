using Abstractions;

namespace Infrastructure.EFPersistence;

public class EfUnitOfWork(AppDbContext dbContext) : IUnitOfWork
{
    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public Task AbortChangesAsync()
    {
        dbContext.ChangeTracker.Clear();
        return Task.CompletedTask;
    }
}