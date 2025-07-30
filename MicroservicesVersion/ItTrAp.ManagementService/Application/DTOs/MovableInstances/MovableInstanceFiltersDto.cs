using ItTrAp.ManagementService.Domain.Enums;

namespace ItTrAp.ManagementService.Application.DTOs.MovableInstances;

public record MovableInstanceFiltersDto
{
    public MovableInstanceStatus? Status { get; set; }
    public uint? LocationId { get; set; }
    public List<uint>? UserIds { get; set; }
}