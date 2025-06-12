using Abstractions;
using Application.MovableInstances.Interfaces;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Infrastructure.Services;

public class MovableInstanceService(
    IMovableItemService movableItemService,
    IMovableInstanceRepository repository,
    IUnitOfWork unitOfWork,
    IQrService qrService) : IMovableInstanceService
{
    public async Task<uint> CreateAsync(uint itemId, CancellationToken ct = default)
    {
        var movableItem = await movableItemService.GetByIdAsync(itemId, ct);
        if (movableItem == null)
        {
            throw new ArgumentException($"Movable item with ID {itemId} does not exist.", nameof(itemId));
        }

        var instance = movableItem.QuickAddInstanceAsync();
        if (instance == null)
        {
            throw new InvalidOperationException("Failed to create a new movable instance.");
        }

        await repository.CreateAsync(instance);

        await unitOfWork.SaveChangesAsync(ct);

        return instance.Id;
    }

    public async Task DeleteAsync(uint itemId, uint id, CancellationToken ct = default)
    {
        var movableItem = await movableItemService.GetByIdAsync(itemId, ct);
        if (movableItem == null)
        {
            throw new ArgumentException($"Movable item with ID {itemId} does not exist.", nameof(itemId));
        }
        if (!movableItem.Instances.Any(i => i.Id == id))
        {
            throw new ArgumentException($"Movable instance with ID {id} does not exist for item with ID {itemId}.", nameof(id));
        }
        await repository.DeleteAsync(id, ct);

        await unitOfWork.SaveChangesAsync(ct);
    }

    public async Task<byte[]> GetQrCodeAsync(uint itemId, uint instanceId, CancellationToken ct = default)
    {
        var movableItem = await movableItemService.GetByIdAsync(itemId, ct);
        if (movableItem == null)
        {
            throw new ArgumentException($"Movable item with ID {itemId} does not exist.", nameof(itemId));
        }

        var instance = await repository.GetByIdAsync(instanceId, ct);
        if (instance == null)
        {
            throw new ArgumentException($"Movable instance with ID {instanceId} does not exist for item with ID {itemId}.", nameof(instanceId));
        }

        return qrService.GetQrCode(QrCodeEntity.MovableInstance, instance.Code);
    }
}