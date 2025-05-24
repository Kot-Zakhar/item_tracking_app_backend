using Application.Categories.Dtos;
using Domain.Categories;

namespace Application.Categories.Interfaces;

public interface ICategoryService
{
    Task<Category> GetByIdAsync(uint id, CancellationToken ct = default);
    Task<uint> CreateAsync(CreateCategoryDto category, CancellationToken ct = default);
    Task UpdateAsync(uint id, UpdateCategoryDto category, CancellationToken ct = default);
    Task DeleteAsync(uint id, CancellationToken ct = default);
}
