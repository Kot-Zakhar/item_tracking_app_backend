using ItTrAp.ManagementService.Application.Interfaces.Services;
using ItTrAp.ManagementService.Infrastructure.Interfaces;
using ItTrAp.ManagementService.Infrastructure.Interfaces.Persistence.Repositories;

namespace ItTrAp.ManagementService.Infrastructure.Services;

public class MovableInstanceService(
    IMovableItemService movableItemService,
    IMovableInstanceRepository repository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : IMovableInstanceService
{
    public async Task<uint> CreateAsync(Guid itemId, uint issuerId, CancellationToken ct = default)
    {
        var movableItem = await movableItemService.GetByIdAsync(itemId, ct);
        if (movableItem == null)
        {
            throw new ArgumentException($"Movable item with ID {itemId} does not exist.", nameof(itemId));
        }

        var issuer = await userRepository.GetByIdAsync(issuerId, ct);
        if (issuer == null)
        {
            throw new ArgumentException($"User with ID {issuerId} does not exist.", nameof(issuerId));
        }

        var instance = movableItem.QuickAddInstance();
        if (instance == null)
        {
            throw new InvalidOperationException("Failed to create a new movable instance.");
        }

        await repository.CreateAsync(instance);

        await unitOfWork.SaveChangesAsync(ct);

        return instance.Id;
    }

    public async Task DeleteAsync(Guid itemId, uint id, uint issuerId, CancellationToken ct = default)
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