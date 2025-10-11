using ItTrAp.QueryService.Domain.Enums;

namespace ItTrAp.QueryService.Application.Filters;

public record MovableInstanceFiltersDto
{
    public MovableInstanceStatus? Status { get; set; }
    public uint? LocationId { get; set; }
    public List<uint>? UserIds { get; set; }
}