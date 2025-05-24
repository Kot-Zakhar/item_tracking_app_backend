using Application.Categories.Dtos;
using Application.Categories.Interfaces;
using Domain.Categories;
using Domain.Categories.Interfaces;
using Infrastructure.Interfaces.Categories;
using Infrastructure.Interfaces.Common;

namespace Infrastructure.Services.Categories;

public class CategoryService(ICategoryRepository repo, IUnitOfWork unitOfWork, Lazy<ICategoryUniquenessChecker> nameUniquenessChecker) : ICategoryService
{
    public async Task<Category> GetByIdAsync(uint id, CancellationToken ct = default)
    {
        var category = await repo.GetByIdAsync(id, ct);
        if (category == null)
            throw new ArgumentException($"Category with ID {id} not found.");

        return category;
    }

    public async Task<uint> CreateAsync(CreateCategoryDto createDto, CancellationToken ct = default)
    {
        Category? parent = null;

        if (createDto.ParentId != null)
        {
            parent = await repo.GetByIdAsync(createDto.ParentId.Value, ct);
            if (parent == null)
                throw new ArgumentException($"Parent category with ID {createDto.ParentId} not found.");
        }

        var category = await Category.CreateAsync(createDto.Name, createDto.Icon, parent, nameUniquenessChecker.Value, ct);

        category = await repo.CreateAsync(category, ct);

        await unitOfWork.SaveChangesAsync(ct);

        return category.Id;
    }

    public async Task UpdateAsync(uint id, UpdateCategoryDto updateData, CancellationToken ct = default)
    {
        var existingCategory = await repo.GetByIdAsync(id, ct);
        if (existingCategory == null)
            throw new ArgumentException($"Category with ID {id} not found.");

        await existingCategory.UpdateAsync(updateData.Name, updateData.Icon, nameUniquenessChecker.Value, ct);

        await repo.UpdateAsync(existingCategory, ct);
        await unitOfWork.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(uint id, CancellationToken ct = default)
    {
        await repo.DeleteAsync(id, ct);
        await unitOfWork.SaveChangesAsync(ct);
    }

}
