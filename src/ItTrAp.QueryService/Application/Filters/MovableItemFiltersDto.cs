using ItTrAp.QueryService.Domain.Enums;

namespace ItTrAp.QueryService.Application.Filters;

// TODO: it was CategoryId with ExcludeItemsOfChildCategories
public record MovableItemFiltersDto
{
    public MovableInstanceStatus? Status { get; init; }
    public List<uint>? CategoryIds { get; init; }
    public List<uint>? LocationIds { get; init; }
    public string? Search { get; init; }
    public List<uint>? UserIds { get; init; }

    public bool HasInstanceFilters => (UserIds != null && UserIds.Count > 0) || (LocationIds != null && LocationIds.Count > 0) || Status != null;
}
