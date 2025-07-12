using ItTrAp.InventoryService.DTOs.Categories;

namespace ItTrAp.InventoryService.Interfaces.Repositories;

public interface ICategoryReadRepository
{
    // Task<CategoryWithDetailsViewModel?> GetByIdAsync(int id, CancellationToken ct = default);
    // Task<List<CategoryWithDetailsViewModel>> GetAllFiltered(string? search, int? top, CancellationToken ct = default);
    Task<List<CategoryWithDetailsDto>> GetCategoryTreeAsync(CancellationToken ct = default);
    Task<CategoryWithDetailsDto?> GetCategoryTreeFromNode(uint id, CancellationToken ct = default);
}