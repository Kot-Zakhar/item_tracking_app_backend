using ItTrAp.QueryService.Domain.Enums;

namespace ItTrAp.QueryService.Application.Filters;

// TODO: it was CategoryId with ExcludeItemsOfChildCategories
public record MovableItemFiltersDto(
    MovableInstanceStatus? Status,
    List<uint>? CategoryIds,
    uint? LocationId,
    string? Search,
    List<uint>? UserIds
);