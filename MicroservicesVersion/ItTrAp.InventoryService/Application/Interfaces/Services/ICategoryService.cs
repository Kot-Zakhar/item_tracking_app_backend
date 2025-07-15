using ItTrAp.InventoryService.Application.DTOs.Categories;
using ItTrAp.InventoryService.Domain.Models;

namespace ItTrAp.InventoryService.Application.Interfaces.Services;

public interface ICategoryService
{
    Task<Category> GetByIdAsync(uint id, CancellationToken ct = default);
    Task<uint> CreateAsync(CreateCategoryDto category, CancellationToken ct = default);
    Task UpdateAsync(uint id, UpdateCategoryDto category, CancellationToken ct = default);
    Task DeleteAsync(uint id, CancellationToken ct = default);
}
