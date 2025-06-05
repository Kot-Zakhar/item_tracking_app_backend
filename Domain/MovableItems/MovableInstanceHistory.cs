using Domain.Locations;
using Domain.Users;

namespace Domain.MovableItems;

public class MovableInstanceHistory
{
    public uint Id { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public virtual required MovableInstance MovableInstance { get; set; }
    public virtual required User User { get; set; }
    public virtual Location? FromLocation { get; set; }
    public virtual Location? ToLocation { get; set; }

    public static MovableInstanceHistory Create(
        MovableInstance movableInstance,
        User user,
        Location? fromLocation,
        Location? toLocation)
    {
        return new MovableInstanceHistory
        {
            MovableInstance = movableInstance,
            User = user,
            FromLocation = fromLocation,
            StartedAt = DateTime.UtcNow,
            ToLocation = toLocation,
            EndedAt = toLocation == null ? null : DateTime.UtcNow,
        };
    }

    public void SetReturnDetails(Location location)
    {
        if (location == null) throw new ArgumentNullException(nameof(location));
        ToLocation = location;
        EndedAt = location == null ? null : DateTime.UtcNow;
    }
}