using ItTrAp.ManagementService.Domain.Enums;

namespace ItTrAp.ManagementService.Application.DTOs.MovableInstances;

public record MovableInstanceFiltersDto
{
    public MovableInstanceStatus? Status { get; set; }
    public List<uint>? LocationIds { get; set; }
    public List<uint>? UserIds { get; set; }
}