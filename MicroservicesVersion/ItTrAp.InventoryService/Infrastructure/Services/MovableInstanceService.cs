using ItTrAp.InventoryService.Application.Interfaces.Repositories;
using ItTrAp.InventoryService.Application.Interfaces.Services;
using ItTrAp.InventoryService.Domain.Aggregates;
using ItTrAp.InventoryService.Domain.Events.MovableInstances;
using ItTrAp.InventoryService.Infrastructure.Interfaces.Persistence;
using ItTrAp.InventoryService.Infrastructure.Interfaces.Persistence.Repositories;
using MediatR;

namespace ItTrAp.InventoryService.Infrastructure.Services;

public class MovableInstanceService(
    IMovableItemRepository itemRepo,
    IMovableInstanceRepository instanceRepo,
    IUnitOfWork unitOfWork,
    IMediator mediator) : IMovableInstanceService
{
    public async Task<uint> CreateAsync(Guid itemId, CancellationToken ct = default)
    {
        var movableItem = await itemRepo.GetByIdAsync(itemId, ct);
        if (movableItem == null)
        {
            throw new ArgumentException($"Movable item with ID {itemId} does not exist.", nameof(itemId));
        }

        var instance = new MovableInstance()
        {
            MovableItem = null!,
        };

        instance = await instanceRepo.CreateAsync(instance, ct);
        if (instance == null)
        {
            throw new InvalidOperationException("Failed to create a new movable instance.");
        }

        instance.MovableItem = movableItem;
        movableItem.MovableInstances.Add(instance);

        await unitOfWork.SaveChangesAsync(ct);

        await mediator.Publish(new MovableInstanceCreated { MovableInstanceId = instance.Id, MovableItemId = instance.MovableItem.Id }, ct);

        return instance.Id;
    }

    public async Task DeleteAsync(Guid itemId, uint id, CancellationToken ct = default)
    {
        // var movableItem = await itemRepo.GetByIdAsync(itemId, ct);
        // if (movableItem == null)
        // {
        //     throw new ArgumentException($"Movable item with ID {itemId} does not exist.", nameof(itemId));
        // }

        var instance = await instanceRepo.GetByIdAsync(id);
        if (instance == null || instance.MovableItem.Id != itemId)
        // var instances = await instanceReadRepo.GetAllAsync(itemId);
        // if (!instances.Any(i => i.Id == id))
        {
            throw new ArgumentException($"Movable instance with ID {id} does not exist for item with ID {itemId}.", nameof(id));
        }
        await instanceRepo.DeleteAsync(id, ct);

        await unitOfWork.SaveChangesAsync(ct);

        await mediator.Publish(new MovableInstanceDeleted { MovableInstanceId = id, MovableItemId = itemId });
    }
}