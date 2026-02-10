using ItTrAp.ManagementService.Application.Interfaces.Services;
using ItTrAp.ManagementService.Domain.Aggregates;
using ItTrAp.ManagementService.Domain.Enums;
using ItTrAp.ManagementService.Infrastructure.Interfaces;
using ItTrAp.ManagementService.Infrastructure.Interfaces.Persistence.Repositories;

namespace ItTrAp.ManagementService.Infrastructure.Services;

public class ReservationService(
    ILocationRepository locationRepo,
    IMovableItemRepository movableItemRepo,
    IMovableInstanceRepository movableInstanceRepo,
    IUserRepository userRepo,
    IUnitOfWork unitOfWork) : IReservationService
{
    public async Task<uint> BookAnyInstanceInLocationAsync(uint issuerId, uint userId, Guid itemId, uint locationId, CancellationToken ct = default)
    {
        var issuer = await GetUserAsync(issuerId, ct);
        var user = await GetUserAsync(userId, ct);
        var item = await GetItemAsync(itemId, ct);
        var location = await GetLocationAsync(locationId, ct);

        var query = movableInstanceRepo.GetAllAsync(ct)
            .Where(i => i.MovableItem.Id == itemId && i.Location != null && i.Location.Id == locationId && i.Status == MovableInstanceStatus.Available);

        var list = await unitOfWork.MaterializeAsync(query, ct);
        if (!list.Any())
            throw new InvalidOperationException($"No available instances found for item {itemId} at location {locationId}.");

        var instance = list.First();

        instance.Book(issuer, user);

        await movableInstanceRepo.UpdateAsync(instance, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return instance.Id;
    }

    public async Task BookAsync(uint issuerId, uint bookerId, uint instanceId, CancellationToken ct = default)
    {
        var issuer = await GetUserAsync(issuerId, ct);
        var user = await GetUserAsync(bookerId, ct);
        var instance = await GetInstanceAsync(instanceId, ct);

        instance.Book(issuer, user);

        await movableInstanceRepo.UpdateAsync(instance, ct);
        await unitOfWork.SaveChangesAsync(ct);
    }

    public async Task CancelBookingAsync(uint issuerId, uint instanceId, CancellationToken ct = default)
    {
        var issuer = await GetUserAsync(issuerId, ct);
        var instance = await GetInstanceAsync(instanceId, ct);

        instance.CancelBooking(issuer);

        await movableInstanceRepo.UpdateAsync(instance, ct);
        await unitOfWork.SaveChangesAsync(ct);
    }

    public async Task AssignAsync(uint issuerId, uint assigneeId, uint instanceId, CancellationToken ct = default)
    {
        var issuer = await GetUserAsync(issuerId, ct);
        var user = await  GetUserAsync(assigneeId, ct);
        var instance = await GetInstanceAsync(instanceId, ct);

        // TODO: Force?
        instance.Take(issuer, user, force: true);

        await movableInstanceRepo.UpdateAsync(instance, ct);
        await unitOfWork.SaveChangesAsync(ct);
    }

    public async Task TakeByCodeAsync(uint issuerId, Guid code, CancellationToken ct = default)
    {
        var issuer = await GetUserAsync(issuerId, ct);
        var instance = await GetInstanceByCodeAsync(code, ct);

        // TODO: looks weird
        instance.Take(issuer: issuer, user: issuer);

        await movableInstanceRepo.UpdateAsync(instance, ct);
        await unitOfWork.SaveChangesAsync(ct);
    }

    // TODO: This method should not exist
    public async Task ReleaseForcefullyAsync(uint issuerId, uint instanceId, uint locationId, CancellationToken ct = default)
    {
        var issuer = await GetUserAsync(issuerId, ct);
        var instance = await GetInstanceAsync(instanceId, ct);
        var location = await GetLocationAsync(locationId, ct);

        instance.Release(issuer, location, force: true);

        await movableInstanceRepo.UpdateAsync(instance, ct);
        await unitOfWork.SaveChangesAsync(ct);
    }

    public async Task ReleaseAsync(uint issuerId, Guid instanceCode, Guid locationCode, CancellationToken ct = default)
    {
        var issuer = await GetUserAsync(issuerId, ct);
        var instance = await GetInstanceByCodeAsync(instanceCode, ct);
        var location = await GetLocationByCodeAsync(locationCode, ct);

        instance.Release(issuer, location);

        await movableInstanceRepo.UpdateAsync(instance, ct);
        await unitOfWork.SaveChangesAsync(ct);
    }

    public async Task MoveAsync(uint issuerId, uint instanceId, uint locationId, CancellationToken ct = default)
    {
        var issuer = await GetUserAsync(issuerId, ct);
        var instance = await GetInstanceAsync(instanceId, ct);
        var location = await GetLocationAsync(locationId, ct);

        instance.Move(issuer, location);

        await movableInstanceRepo.UpdateAsync(instance, ct);
        await unitOfWork.SaveChangesAsync(ct);
    }

    // TODO: This method should not exist
    // It's an old version of API
    public async Task MoveOrReleaseAsync(uint issuerId, uint instanceId, uint? locationId, CancellationToken ct = default)
    {
        var issuer = await GetUserAsync(issuerId, ct);
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
            instance.Move(issuer, location);
        }
        else
        {
            instance.Release(issuer, location);
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

    private async Task<MovableItem> GetItemAsync(Guid itemId, CancellationToken ct = default)
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