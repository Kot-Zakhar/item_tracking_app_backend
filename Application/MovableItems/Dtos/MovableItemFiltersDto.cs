using Domain.MovableItems;

namespace Application.MovableItems.Dtos;
// TODO: it was CategoryId with ExcludeItemsOfChildCategories
public record MovableItemFiltersDto(
    MovableInstanceStatus? Status,
    List<uint>? CategoryIds,
    uint? LocationId,
    string? Search,
    List<uint>? UserIds
);