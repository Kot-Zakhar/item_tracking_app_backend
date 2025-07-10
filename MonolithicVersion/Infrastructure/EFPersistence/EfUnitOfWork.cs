using Domain.Aggregates;
using Infrastructure.Interfaces.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EFPersistence;

public class EfUnitOfWork(AppDbContext dbContext, IMediator mediator) : IUnitOfWork
{
    public async Task<bool> SaveChangesAsync(CancellationToken ct = default)
    {
        var result = await dbContext.SaveChangesAsync(ct) > 0;

        var aggregates = dbContext.ChangeTracker
            .Entries<AggregateBase>()
            .Select(e => e.Entity)
            .Where(a => a.DomainEvents.Any())
            .ToList();

        foreach (var aggregate in aggregates)
        {
            foreach (var domainEvent in aggregate.DomainEvents)
            {
                await mediator.Publish(domainEvent, ct);
            }
            aggregate.ClearEvents();
        }

        return result;
    }

    public Task AbortChangesAsync(CancellationToken ct = default)
    {
        dbContext.ChangeTracker.Clear();

        var aggregates = dbContext.ChangeTracker
            .Entries<AggregateBase>()
            .Select(e => e.Entity)
            .Where(a => a.DomainEvents.Any())
            .ToList();

        foreach (var aggregate in aggregates)
        {
            aggregate.ClearEvents();
        }

        return Task.CompletedTask;
    }

    public async Task<List<TEntity>> MaterializeAsync<TEntity>(IQueryable<TEntity> query, CancellationToken ct = default)
    {
        return await query.ToListAsync(ct);
    }
}