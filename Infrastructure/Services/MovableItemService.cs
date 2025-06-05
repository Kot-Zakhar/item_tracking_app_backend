using Abstractions;
using Application.Categories.Interfaces;
using Application.MovableItems.DTOs;
using Application.MovableItems.Interfaces;
using Domain.MovableItems;
using Domain.MovableItems.Interfaces;
using Infrastructure.EFPersistence;

namespace Infrastructure.Services;

public class MovableItemService(
    ICategoryService categoryService,
    IRepository<MovableItem> repo,
    Lazy<IMovableItemUniquenessChecker> nameUniquenessChecker,
    AppDbContext context) : IMovableItemService
{
    public async Task<uint> CreateAsync(CreateMovableItemDto data, CancellationToken ct = default)
    {
        var category = await categoryService.GetByIdAsync(data.CategoryId, ct);
        var movableItem = await MovableItem.CreateAsync(data.Name, data.Description, data.ImageUrl, category, nameUniquenessChecker.Value, ct);
        movableItem = await repo.CreateAsync(movableItem, ct);
        await context.SaveChangesAsync(ct);
        return movableItem.Id;
    }

    public async Task<MovableItem?> GetByIdAsync(uint itemId, CancellationToken ct = default)
    {
        return await repo.GetByIdAsync(itemId, ct);
    }

    public async Task UpdateAsync(uint id, UpdateMovableItemDto data, CancellationToken ct = default)
    {
        var movableItem = await repo.GetByIdAsync(id, ct);
        if (movableItem == null)
            throw new ArgumentException($"MovableItem with ID {id} not found.");

        Category? category = null;
        if (data.CategoryId.HasValue)
        {
            category = await categoryService.GetByIdAsync(data.CategoryId.Value, ct);
        }

        await movableItem.UpdateAsync(data.Name, data.Description, data.ImageUrl, category, nameUniquenessChecker.Value, ct);
        await repo.UpdateAsync(movableItem, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(uint id, CancellationToken ct = default)
    {
        await repo.DeleteAsync(id, ct);
        await context.SaveChangesAsync(ct);
    }
}