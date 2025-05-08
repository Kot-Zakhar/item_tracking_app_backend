using Domain.MovableItems;

namespace Domain.Locations;

public class Location
{
    public uint Id { get; set; }
    public Guid Code { get; set; }
    public sbyte Floor { get; set; }
    public required string Title { get; set; }
    public string? Department { get; set; }
    public DateTime CreatedAt { get; set; }
    public virtual required List<MovableInstance> Instances { get; set; } = new();
}