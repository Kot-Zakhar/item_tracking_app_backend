namespace ItTrAp.InventoryService.Application.DTOs.MovableInstances;

public class MovableInstanceDto
{
    public uint Id { get; set; }
    public Guid MovableItemId { get; set; }
    public DateTime CreatedAt { get; set; }
}