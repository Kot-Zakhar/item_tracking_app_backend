using Domain.Enums;
using Domain.Models;

namespace Domain.Services;

public static class MovableInstanceStateManagementService
{
    public static void BookInstance(
        MovableInstance movableInstance,
        User user)
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

        if (movableInstance.Status != MovableInstanceStatus.Available)
        {
            throw new InvalidOperationException("Movable instance is not available for booking.");
        }

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
        MovableInstance movableInstance,
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

        var location = movableInstance.Location;

        movableInstance.Status = MovableInstanceStatus.Taken;
        movableInstance.User = user;
        movableInstance.Location = null;
        
        location?.Instances.Remove(movableInstance);
        user.MovableInstances.Add(movableInstance);

        movableInstance.History.Add(MovableInstanceHistory.Create(movableInstance, user, movableInstance.Location, null));
    }

    public static void CancelBooking(
        MovableInstance movableInstance,
        User user,
        bool force = false)
    {
        if (movableInstance == null) throw new ArgumentNullException(nameof(movableInstance));
        if (user == null) throw new ArgumentNullException(nameof(user));

        if (movableInstance.Status != MovableInstanceStatus.Booked)
        {
            throw new InvalidOperationException("Movable instance is not booked.");
        }

        if (movableInstance.User?.Id != user.Id && !force)
        {
            throw new InvalidOperationException("User is not the one who booked this item.");
        }

        movableInstance.Status = MovableInstanceStatus.Available;
        movableInstance.User = null;

        user.MovableInstances.Remove(movableInstance);
    }

    public static void ReleaseInstance(
        MovableInstance movableInstance,
        User user,
        Location newLocation,
        bool force = false)
    {
        if (movableInstance == null) throw new ArgumentNullException(nameof(movableInstance));
        if (newLocation == null) throw new ArgumentNullException(nameof(newLocation));
        if (user == null) throw new ArgumentNullException(nameof(user));

        if (movableInstance.Status != MovableInstanceStatus.Taken)
        {
            throw new InvalidOperationException("Movable instance is not taken.");
        }

        if (movableInstance.User?.Id != user.Id && !force)
        {
            throw new InvalidOperationException("User is not the one who took this item.");
        }

        movableInstance.Status = MovableInstanceStatus.Available;
        movableInstance.User = null;
        movableInstance.Location = newLocation;

        user.MovableInstances.Remove(movableInstance);
        newLocation.Instances.Add(movableInstance);

        var historyRecord = movableInstance.History
            .OrderByDescending(x => x.StartedAt)
            .FirstOrDefault();

        if (historyRecord == null)
        {
            movableInstance.History.Add(
                MovableInstanceHistory.Create(movableInstance, user, null, newLocation));
        }
        else
        {
            historyRecord.SetReturnDetails(newLocation);
        }
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

        var fromLocation = movableInstance.Location;

        if (fromLocation != null && fromLocation.Id != newLocation.Id)
        {
            fromLocation.Instances.Remove(movableInstance);
        }

        movableInstance.Location = newLocation;

        var historyRecord = movableInstance.History
            .OrderByDescending(x => x.StartedAt)
            .FirstOrDefault();

        if (historyRecord == null)
        {
            movableInstance.History.Add(
                MovableInstanceHistory.Create(movableInstance, user, fromLocation, newLocation));
        }
        else
        {
            historyRecord.SetReturnDetails(newLocation);
        }
    }
}