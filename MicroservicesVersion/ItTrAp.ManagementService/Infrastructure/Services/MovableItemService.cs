using ItTrAp.ManagementService.Application.Interfaces.Services;
using ItTrAp.ManagementService.Domain.Aggregates;
using ItTrAp.ManagementService.Infrastructure.Interfaces;
using ItTrAp.ManagementService.Infrastructure.Interfaces.Persistence.Repositories;

namespace ItTrAp.ManagementService.Infrastructure.Services;

public class MovableItemService(IMovableItemRepository repo, IUnitOfWork unitOfWork) : IMovableItemService
{

    public async Task<MovableItem?> GetByIdAsync(Guid itemId, CancellationToken ct = default)
    {
        return await repo.GetByIdAsync(itemId, ct);
    }

    public async Task CreateAsync(Guid MovableItemId, CancellationToken cancellationToken)
    {
        var MovableItem = new MovableItem { Id = MovableItemId };
        await repo.CreateAsync(MovableItem);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid MovableItemId, CancellationToken cancellationToken)
    {
        var MovableItem = await repo.GetByIdAsync(MovableItemId);
        if (MovableItem != null)
        {
            await repo.DeleteAsync(MovableItemId, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}