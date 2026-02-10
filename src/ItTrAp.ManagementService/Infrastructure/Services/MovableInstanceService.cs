using ItTrAp.ManagementService.Application.Interfaces.Services;
using ItTrAp.ManagementService.Domain.Aggregates;
using ItTrAp.ManagementService.Infrastructure.Interfaces;
using ItTrAp.ManagementService.Infrastructure.Interfaces.Persistence.Repositories;

namespace ItTrAp.ManagementService.Infrastructure.Services;

public class MovableInstanceService(
    IMovableItemService movableItemService,
    IMovableInstanceRepository repository,
    IUnitOfWork unitOfWork) : IMovableInstanceService
{
    public async Task CreateAsync(Guid itemId, uint id, CancellationToken ct = default)
    {
        var movableItem = await movableItemService.GetByIdAsync(itemId, ct);
        if (movableItem == null)
        {
            throw new ArgumentException($"Movable item with ID {itemId} does not exist.", nameof(itemId));
        }

        var instance = MovableInstance.Create(movableItem, id);
        if (instance == null)
        {
            throw new InvalidOperationException("Failed to create a new movable instance.");
        }

        instance.Id = id;

        await repository.CreateAsync(instance, ct);

        await unitOfWork.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid itemId, uint id, CancellationToken ct = default)
    {
        var movableItem = await movableItemService.GetByIdAsync(itemId, ct);
        if (movableItem == null)
        {
            throw new ArgumentException($"Movable item with ID {itemId} does not exist.", nameof(itemId));
        }
        if (!movableItem.MovableInstances.Any(i => i.Id == id))
        {
            throw new ArgumentException($"Movable instance with ID {id} does not exist for item with ID {itemId}.", nameof(id));
        }
        await repository.DeleteAsync(id, ct);

        await unitOfWork.SaveChangesAsync(ct);
    }
}