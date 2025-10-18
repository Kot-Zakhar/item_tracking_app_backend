using ItTrAp.QueryService.Domain.Enums;

namespace ItTrAp.QueryService.Infrastructure.DTOs;

public class MovableInstanceStatusDto
{
    public uint Id { get; set; }
    public Guid ItemId { get; set; }
    public MovableInstanceStatus Status { get; set; }
    public uint? LocationId { get; set; }
    public uint? UserId { get; set; }
}