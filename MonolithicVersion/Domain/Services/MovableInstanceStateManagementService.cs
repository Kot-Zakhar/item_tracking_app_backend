using Domain.Aggregates.MovableInstances;
using Domain.Aggregates.MovableItems;
using Domain.Aggregates.Users;
using Domain.Aggregates.Locations;
using Domain.Enums;

namespace Domain.Services;

public static class MovableInstanceStateManagementService
{
    public static void BookInstance(
        MovableInstance movableInstance,
        User user,
        bool force = false)
    {
        if (movableInstance == null) throw new ArgumentNullException(nameof(movableInstance));
        if (user == null) throw new ArgumentNullException(nameof(user));

        if (user.MovableInstances.Any(x => x.Id == movableInstance.Id))
        {
            throw new InvalidOperationException("User already has this item.");
        }

        if (user.IsMaxMovableInstancesReached())
        {
            throw new InvalidOperationException("User has reached the maximum number of movable instances.");
        }

        if (movableInstance.Status != MovableInstanceStatus.Available && !force)
        {
            throw new InvalidOperationException("Movable instance is not available for booking.");
        }

        movableInstance.User?.MovableInstances.Remove(movableInstance);

        movableInstance.Status = MovableInstanceStatus.Booked;
        movableInstance.User = user;

        user.MovableInstances.Add(movableInstance);
    }

    public static MovableInstance BookAnyInstanceInLocation(
        MovableItem movableItem,
        Location location,
        User user)
    {
        if (movableItem == null) throw new ArgumentNullException(nameof(movableItem));
        if (location == null) throw new ArgumentNullException(nameof(location));
        if (user == null) throw new ArgumentNullException(nameof(user));

        var availableInstance = movableItem.Instances
            .Where(x => x.Location?.Id == location.Id && x.Status == MovableInstanceStatus.Available)
            .FirstOrDefault();

        if (availableInstance == null)
        {
            throw new InvalidOperationException("No available instance found in the specified location.");
        }

        BookInstance(availableInstance, user);

        return availableInstance;
    }

    public static void TakeInstance(
        this MovableInstance movableInstance,
        User user,
        bool force = false)
    {
        if (movableInstance == null) throw new ArgumentNullException(nameof(movableInstance));
        if (user == null) throw new ArgumentNullException(nameof(user));

        if (user.MovableInstances.Any(x => x.Id == movableInstance.Id && x.Status == MovableInstanceStatus.Taken))
        {
            throw new InvalidOperationException("User already has this item.");
        }

        if (user.IsMaxMovableInstancesReached())
        {
            throw new InvalidOperationException("User has reached the maximum number of movable instances.");
        }

        if (movableInstance.Status == MovableInstanceStatus.Taken && !force)
        {
            throw new InvalidOperationException("Movable instance is not available for taking.");
        }
        
        if (movableInstance.Status == MovableInstanceStatus.Booked && user.Id != movableInstance.User?.Id && !force)
        {
            throw new InvalidOperationException("Movable instance is booked and cannot be taken.");
        }

        movableInstance.User?.MovableInstances.Remove(movableInstance);

        var location = movableInstance.Location;

        movableInstance.Status = MovableInstanceStatus.Taken;
        movableInstance.User = user;
        movableInstance.Location = null;
        
        user.MovableInstances.Add(movableInstance);
    }

    public static void CancelBooking(
        MovableInstance movableInstance,
        User issuer,
        bool force = false)
    {
        if (movableInstance == null) throw new ArgumentNullException(nameof(movableInstance));
        if (!force && issuer == null) throw new ArgumentNullException(nameof(issuer));

        if (movableInstance.Status != MovableInstanceStatus.Booked)
        {
            throw new InvalidOperationException("Movable instance is not booked.");
        }

        if (!force && movableInstance.User?.Id != issuer.Id)
        {
            throw new InvalidOperationException("User is not the one who booked this item.");
        }

        movableInstance.User?.MovableInstances.Remove(movableInstance);

        movableInstance.Status = MovableInstanceStatus.Available;
        movableInstance.User = null;
    }

    public static void ReleaseInstance(
        MovableInstance movableInstance,
        User issuer,
        Location newLocation,
        bool force = false)
    {
        if (movableInstance == null) throw new ArgumentNullException(nameof(movableInstance));
        if (newLocation == null) throw new ArgumentNullException(nameof(newLocation));
        if (!force && issuer == null) throw new ArgumentNullException(nameof(issuer));

        if (movableInstance.Status != MovableInstanceStatus.Taken)
        {
            throw new InvalidOperationException("Movable instance is not taken.");
        }

        if (!force && movableInstance.User?.Id != issuer!.Id)
        {
            throw new InvalidOperationException("User is not the one who took this item.");
        }

        var currentHolder = movableInstance.User;

        movableInstance.Status = MovableInstanceStatus.Available;
        movableInstance.User = null;
        movableInstance.Location = newLocation;

        currentHolder?.MovableInstances.Remove(movableInstance);
    }

    public static void MoveInstance(
        MovableInstance movableInstance,
        Location? newLocation,
        User user,
        bool force = false)
    {
        if (movableInstance == null) throw new ArgumentNullException(nameof(movableInstance));
        if (newLocation == null) throw new ArgumentNullException(nameof(newLocation));
        if (user == null) throw new ArgumentNullException(nameof(user));

        if (movableInstance.Status == MovableInstanceStatus.Taken && !force)
        {
            throw new InvalidOperationException("Movable instance is taken.");
        }

        movableInstance.Location = newLocation;
    }
}