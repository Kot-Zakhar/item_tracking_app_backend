using Domain.Locations;
using Domain.Users;

namespace Domain.MovableItems;

public class MovableInstance
{
    public uint Id { get; set; }
    public uint MovableItemId { get; set; }
    public Guid Code { get; set; }
    public MovableInstanceStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }

    public virtual required MovableItem MovableItem { get; set; } 
    public virtual Location? Location { get; set; }
    public virtual User? User { get; set; }

}
