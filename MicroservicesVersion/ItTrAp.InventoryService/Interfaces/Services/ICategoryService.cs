using ItTrAp.InventoryService.DTOs.Categories;
using ItTrAp.InventoryService.Models;

namespace ItTrAp.InventoryService.Interfaces.Services;

public interface ICategoryService
{
    Task<Category> GetByIdAsync(uint id, CancellationToken ct = default);
    Task<uint> CreateAsync(CreateCategoryDto category, CancellationToken ct = default);
    Task UpdateAsync(uint id, UpdateCategoryDto category, CancellationToken ct = default);
    Task DeleteAsync(uint id, CancellationToken ct = default);
}
