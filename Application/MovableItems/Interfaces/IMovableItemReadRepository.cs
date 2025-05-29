using Application.Common.DTOs;
using Application.MovableItems.DTOs;

namespace Application.MovableItems.Interfaces;

public interface IMovableItemReadRepository
{
    Task<List<MovableItemWithDetailsDto>> GetAllFilteredWithDetailsAsync(
        MovableItemFiltersDto filters,
        CancellationToken ct = default
    );

    Task<List<MovableItemWithAmountsByStatusDto>> GetAllWithAmountPerStatusAsync(
        MovableItemFiltersDto filters,
        CancellationToken ct = default
    );

    Task<MovableItemDto?> GetByIdAsync(uint id, CancellationToken ct = default);
}
