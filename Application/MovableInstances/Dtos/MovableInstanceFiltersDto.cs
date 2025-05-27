using Domain.MovableItems;

namespace Application.MovableInstances.Dtos;

public record MovableInstanceFiltersDto
{
    public MovableInstanceStatus? Status { get; set; }
    public uint? LocationId { get; set; }
    public List<uint>? UserIds { get; set; }
}