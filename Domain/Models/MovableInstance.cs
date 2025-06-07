namespace Domain.Models;

public class MovableInstance
{
    public uint Id { get; set; }
    public Guid Code { get; set; }
    public MovableInstanceStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }

    public virtual required MovableItem MovableItem { get; set; }
    public virtual Location? Location { get; set; }
    public virtual User? User { get; set; }
    public virtual List<MovableInstanceHistory> History { get; set; } = new();

    public static MovableInstance Create(MovableItem movableItem)
    {
        return new MovableInstance
        {
            MovableItem = movableItem,
            Code = Guid.NewGuid(),
            Status = MovableInstanceStatus.Available,
            CreatedAt = DateTime.UtcNow
        };
    }
}
