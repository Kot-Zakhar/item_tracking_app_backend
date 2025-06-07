using Abstractions;
using Abstractions.Users;
using Application.Reservations.Interfaces;
using Domain.Models;
using Domain.Services;
using Infrastructure.Interfaces;

namespace Infrastructure.Services;

public class ReservationService(
    ILocationRepository locationRepo,
    IRepository<MovableItem> movableItemRepo,
    IMovableInstanceRepository movableInstanceRepo,
    IUserRepository userRepo,
    IUnitOfWork unitOfWork) : IReservationService
{
    public async Task<uint> BookAnyInstanceInLocationAsync(uint userId, uint itemId, uint locationId, CancellationToken ct = default)
    {
        var user = await GetUserAsync(userId, ct);
        var item = await GetItemAsync(itemId, ct);
        var location = await GetLocationAsync(locationId, ct);

        var query = movableInstanceRepo.GetAllAsync(ct)
            .Where(i => i.MovableItem.Id == itemId && i.Location != null && i.Location.Id == locationId && i.Status == MovableInstanceStatus.Available);

        var list = await unitOfWork.MaterializeAsync(query, ct);
        if (!list.Any())
            throw new InvalidOperationException($"No available instances found for item {itemId} at location {locationId}.");

        var instance = list.First();

        MovableInstanceStateManagementService.BookInstance(instance, user);

        await movableInstanceRepo.UpdateAsync(instance, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return instance.Id;
    }

    public async Task BookAsync(uint userId, uint instanceId, CancellationToken ct = default)
    {
        var user = await GetUserAsync(userId, ct);
        var instance = await GetInstanceAsync(instanceId, ct);

        MovableInstanceStateManagementService.BookInstance(instance, user);

        await movableInstanceRepo.UpdateAsync(instance, ct);
        await unitOfWork.SaveChangesAsync(ct);
    }

    public async Task CancelBookingAsync(uint userId, uint instanceId, CancellationToken ct = default)
    {
        var user = await GetUserAsync(userId, ct);
        var instance = await GetInstanceAsync(instanceId, ct);

        MovableInstanceStateManagementService.CancelBooking(instance, user);

        await movableInstanceRepo.UpdateAsync(instance, ct);
        await unitOfWork.SaveChangesAsync(ct);
    }

    public async Task TakeByCodeAsync(uint userId, Guid code, CancellationToken ct = default)
    {
        var user = await GetUserAsync(userId, ct);
        var instance = await GetInstanceByCodeAsync(code, ct);

        MovableInstanceStateManagementService.TakeInstance(instance, user);

        await movableInstanceRepo.UpdateAsync(instance, ct);
        await unitOfWork.SaveChangesAsync(ct);
    }

    public async Task ReleaseAsync(uint userId, uint instanceId, uint locationId, CancellationToken ct = default)
    {
        var user = await GetUserAsync(userId, ct);
        var instance = await GetInstanceAsync(instanceId, ct);
        var location = await GetLocationAsync(locationId, ct);

        MovableInstanceStateManagementService.ReleaseInstance(instance, user, location);

        await movableInstanceRepo.UpdateAsync(instance, ct);
        await unitOfWork.SaveChangesAsync(ct);
    }

    public async Task ReleaseAsync(uint userId, Guid instanceCode, Guid locationCode, CancellationToken ct = default)
    {
        var user = await GetUserAsync(userId, ct);
        var instance = await GetInstanceByCodeAsync(instanceCode, ct);
        var location = await GetLocationByCodeAsync(locationCode, ct);

        MovableInstanceStateManagementService.ReleaseInstance(instance, user, location);

        await movableInstanceRepo.UpdateAsync(instance, ct);
        await unitOfWork.SaveChangesAsync(ct);
    }

    public async Task TakeAsync(uint userId, uint instanceId, CancellationToken ct = default)
    {
        var user = await  GetUserAsync(userId, ct);
        var instance = await GetInstanceAsync(instanceId, ct);

        MovableInstanceStateManagementService.TakeInstance(instance, user);

        await movableInstanceRepo.UpdateAsync(instance, ct);
        await unitOfWork.SaveChangesAsync(ct);
    }

    public async Task MoveAsync(uint userId, uint instanceId, uint locationId, CancellationToken ct = default)
    {
        var user = await GetUserAsync(userId, ct);
        var instance = await GetInstanceAsync(instanceId, ct);
        var location = await GetLocationAsync(locationId, ct);

        MovableInstanceStateManagementService.MoveInstance(instance, location, user);

        await movableInstanceRepo.UpdateAsync(instance, ct);
        await unitOfWork.SaveChangesAsync(ct);
    }

    // TODO: This method should not exist
    // It's an old version of API
    public async Task MoveOrReleaseAsync(uint userId, uint instanceId, uint? locationId, CancellationToken ct = default)
    {
        var user = await GetUserAsync(userId, ct);
        var instance = await GetInstanceAsync(instanceId, ct);

        Location location;
        if (locationId == null)
        {
            if (instance.Location == null)
                throw new InvalidOperationException("Instance has no location to be released or moved to.");
            location = instance.Location;
        }
        else
        {
            location = await GetLocationAsync(locationId.Value, ct);
        }

        if (instance.Status != MovableInstanceStatus.Taken)
        {
            MovableInstanceStateManagementService.MoveInstance(instance, location, user);
        }
        else
        {
            MovableInstanceStateManagementService.ReleaseInstance(instance, user, location);
        }

        await movableInstanceRepo.UpdateAsync(instance, ct);
        await unitOfWork.SaveChangesAsync(ct);
    }


    private async Task<User> GetUserAsync(uint userId, CancellationToken ct)
    {
        var user = await userRepo.GetByIdAsync(userId, ct);
        if (user == null)
            throw new ArgumentException($"User with ID {userId} not found.");
        return user;
    }

    private async Task<MovableInstance> GetInstanceAsync(uint instanceId, CancellationToken ct)
    {
        var instance = await movableInstanceRepo.GetByIdAsync(instanceId, ct);
        if (instance == null)
            throw new ArgumentException($"Movable instance with ID {instanceId} not found.");
        return instance;
    }

    private async Task<MovableInstance> GetInstanceByCodeAsync(Guid code, CancellationToken ct)
    {
        var instance = await movableInstanceRepo.GetByCodeAsync(code, ct);
        if (instance == null)
            throw new ArgumentException($"Movable instance with code {code} not found.");
        return instance;
    }

    private async Task<MovableItem> GetItemAsync(uint itemId, CancellationToken ct = default)
    {
        var item = await movableItemRepo.GetByIdAsync(itemId, ct);
        if (item == null)
            throw new ArgumentException($"Movable item with ID {itemId} not found.");
        return item;
    }

    private async Task<Location> GetLocationAsync(uint locationId, CancellationToken ct = default)
    {
        var location = await locationRepo.GetByIdAsync(locationId, ct);
        if (location == null)
            throw new ArgumentException($"Location with ID {locationId} not found.");
        return location;
    }

    private async Task<Location> GetLocationByCodeAsync(Guid code, CancellationToken ct = default)
    {
        var location = await locationRepo.GetByCodeAsync(code, ct);
        if (location == null)
            throw new ArgumentException($"Location with code {code} not found.");
        return location;
    }
}