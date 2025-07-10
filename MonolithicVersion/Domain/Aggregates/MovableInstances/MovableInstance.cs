using Domain.Aggregates.Locations;
using Domain.Aggregates.MovableItems;
using Domain.Aggregates.Users;
using Domain.Enums;
using Domain.Events;

namespace Domain.Aggregates.MovableInstances;

public class MovableInstance : AggregateBase
{
    public uint Id { get; set; }
    public Guid Code { get; set; }
    public MovableInstanceStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }

    public virtual required MovableItem MovableItem { get; set; }
    public virtual Location? Location { get; set; }
    public virtual User? User { get; set; }

    public static MovableInstance Create(MovableItem movableItem, User issuer)
    {
        var instance = new MovableInstance
        {
            MovableItem = movableItem,
            Code = Guid.NewGuid(),
            Status = MovableInstanceStatus.Available,
            CreatedAt = DateTime.UtcNow
        };

        instance.Create(issuer);

        return instance;
    }

    protected void Create(User issuer)
    {
        RaiseEvent(new MovableInstanceAdded(Id, Code, MovableItem.Id, Code, issuer.Id));
    }

    public void Book(
        User issuer,
        User user,
        bool force = false)
    {
        if (issuer == null) throw new ArgumentNullException(nameof(issuer));
        if (user == null) throw new ArgumentNullException(nameof(user));

        if (user.MovableInstances.Any(x => x.Id == Id))
        {
            throw new InvalidOperationException("User already has this item.");
        }

        if (user.IsMaxMovableInstancesReached())
        {
            throw new InvalidOperationException("User has reached the maximum number of movable instances.");
        }

        if (Status != MovableInstanceStatus.Available && !force)
        {
            throw new InvalidOperationException("Movable instance is not available for booking.");
        }

        if (Location == null)
        {
            throw new InvalidOperationException("Movable instance must be in a location to be taken.");
        }

        User?.MovableInstances.Remove(this);

        Status = MovableInstanceStatus.Booked;
        User = user;

        user.MovableInstances.Add(this);

        // TODO: potentially overbooked if force

        RaiseEvent(new MovableInstanceBooked(Id, Code, Location.Id, Location.Code, issuer.Id, user.Id));
    }

    public void Take(
        User issuer,
        User user,
        bool force = false)
    {
        if (issuer == null) throw new ArgumentNullException(nameof(issuer));
        if (user == null) throw new ArgumentNullException(nameof(user));

        if (user.MovableInstances.Any(x => x.Id == Id && x.Status == MovableInstanceStatus.Taken))
        {
            throw new InvalidOperationException("User already has this item.");
        }

        if (user.IsMaxMovableInstancesReached())
        {
            throw new InvalidOperationException("User has reached the maximum number of movable instances.");
        }

        if (Status == MovableInstanceStatus.Taken && !force)
        {
            throw new InvalidOperationException("Movable instance is not available for taking.");
        }

        if (Status == MovableInstanceStatus.Booked && user.Id != User?.Id && !force)
        {
            throw new InvalidOperationException("Movable instance is booked and cannot be taken.");
        }

        if (Location == null)
        {
            throw new InvalidOperationException("Movable instance must be in a location to be taken.");
        }

        User?.MovableInstances.Remove(this);

        var location = Location;

        Status = MovableInstanceStatus.Taken;
        User = user;
        Location = null;

        user.MovableInstances.Add(this);

        // TODO: potentially overtaken if force

        RaiseEvent(new MovableInstanceTaken(Id, Code, location.Id, location.Code, issuer.Id, user.Id));
    }

    public void CancelBooking(
        User issuer,
        bool force = false)
    {
        if (issuer == null) throw new ArgumentNullException(nameof(issuer));

        if (User == null) throw new InvalidOperationException("Movable instance is not booked by any user.");

        if (Status != MovableInstanceStatus.Booked)
        {
            throw new InvalidOperationException("Movable instance is not booked.");
        }

        if (!force && User?.Id != issuer.Id)
        {
            throw new InvalidOperationException("User is not the one who booked this item.");
        }

        var currentHolder = User;

        currentHolder!.MovableInstances.Remove(this);

        Status = MovableInstanceStatus.Available;
        User = null;

        RaiseEvent(new MovableInstanceBookingCancelled(Id, Code, issuer.Id, currentHolder!.Id));
    }

    public void Release(
        User issuer,
        Location newLocation,
        bool force = false)
    {
        if (newLocation == null) throw new ArgumentNullException(nameof(newLocation));
        if (issuer == null) throw new ArgumentNullException(nameof(issuer));

        if (User == null) throw new InvalidOperationException("Movable instance is not taken by any user.");

        if (Status != MovableInstanceStatus.Taken)
        {
            throw new InvalidOperationException("Movable instance is not taken.");
        }

        if (!force && User?.Id != issuer!.Id)
        {
            throw new InvalidOperationException("User is not the one who took this item.");
        }

        var currentHolder = User;

        Status = MovableInstanceStatus.Available;
        User = null;
        Location = newLocation;

        currentHolder!.MovableInstances.Remove(this);

        RaiseEvent(new MovableInstanceReturned(Id, Code, newLocation.Id, newLocation.Code, issuer.Id, currentHolder!.Id));
    }

    public void Move(
        User issuer,
        Location? newLocation,
        bool force = false)
    {
        if (newLocation == null) throw new ArgumentNullException(nameof(newLocation));
        if (issuer == null) throw new ArgumentNullException(nameof(issuer));

        if (Status == MovableInstanceStatus.Taken && !force)
        {
            throw new InvalidOperationException("Movable instance is taken.");
        }

        var currentLocation = Location;

        Location = newLocation;
        
        RaiseEvent(new MovableInstanceMoved(Id, Code, currentLocation?.Id, currentLocation?.Code, newLocation.Id, newLocation.Code, issuer.Id));
    }
}
