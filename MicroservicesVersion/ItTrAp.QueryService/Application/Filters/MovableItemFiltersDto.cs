using ItTrAp.QueryService.Domain.Enums;

namespace ItTrAp.QueryService.Application.Filters;

// TODO: it was CategoryId with ExcludeItemsOfChildCategories
public record MovableItemFiltersDto
{
    public MovableInstanceStatus? Status { get; init; }
    public List<uint>? CategoryIds { get; init; }
    public uint? LocationId { get; init; }
    public string? Search { get; init; }
    public List<uint>? UserIds { get; init; }
}
