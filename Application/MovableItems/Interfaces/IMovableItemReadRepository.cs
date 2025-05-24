using Application.MovableItems.ViewModels;
using Application.Common.ViewModels;
using Application.MovableItems.Dtos;

namespace Application.MovableItems.Interfaces;

public interface IMovableItemReadRepository
{
    Task<List<MovableItemWithDetailsViewModel>> GetAllFilteredWithDetailsAsync(
        MovableItemFiltersDto filters,
        CancellationToken ct = default
    );

    Task<List<MovableItemWithAmountsByStatusViewModel>> GetAllWithAmountPerStatusAsync(
        MovableItemFiltersDto filters,
        CancellationToken ct = default
    );

    Task<MovableItemViewModel?> GetByIdAsync(uint id, CancellationToken ct = default);
}
