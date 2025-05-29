using Domain.MovableItems;

namespace Application.MovableItems.DTOs;
// TODO: it was CategoryId with ExcludeItemsOfChildCategories
public record MovableItemFiltersDto(
    MovableInstanceStatus? Status,
    List<uint>? CategoryIds,
    uint? LocationId,
    string? Search,
    List<uint>? UserIds
);