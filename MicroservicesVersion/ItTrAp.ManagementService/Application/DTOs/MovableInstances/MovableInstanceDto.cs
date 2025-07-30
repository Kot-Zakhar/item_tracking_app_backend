using ItTrAp.ManagementService.Domain.Enums;

namespace ItTrAp.ManagementService.Application.DTOs.MovableInstances;

public class MovableInstanceDto
{
    public uint Id { get; set; }
    public Guid MovableItemId { get; set; }
    public Guid Code { get; set; }
    public MovableInstanceStatus Status { get; set; }
    public uint? LocationId { get; set; }
    public uint? UserId { get; set; }
    public DateTime CreatedAt { get; set; }
}