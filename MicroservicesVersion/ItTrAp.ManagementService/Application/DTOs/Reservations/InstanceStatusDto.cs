using ItTrAp.ManagementService.Domain.Enums;

namespace ItTrAp.ManagementService.Application.DTOs.Reservations;

public class InstanceStatusDto
{
    public uint Id { get; set; }
    public Guid ItemId { get; set; }
    public MovableInstanceStatus Status { get; set; }
    public uint? UserId { get; set; }
    public uint? LocationId { get; set; }
}