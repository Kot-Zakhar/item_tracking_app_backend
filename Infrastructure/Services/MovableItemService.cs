using Abstractions;
using Application.Categories.Interfaces;
using Application.MovableItems.DTOs;
using Domain.Models;
using Domain.Interfaces;
using Infrastructure.EFPersistence;
using Application.Files.Interfaces;

namespace Infrastructure.Services;

public class MovableItemService(
    ICategoryService categoryService,
    IRepository<MovableItem> repo,
    Lazy<IMovableItemUniquenessChecker> nameUniquenessChecker,
    Lazy<IFileService> fileService,
    AppDbContext context) : Application.MovableItems.Interfaces.IMovableItemService, Infrastructure.Interfaces.IMovableItemService
{
    public async Task<uint> CreateAsync(CreateMovableItemDto data, CancellationToken ct = default)
    {
        var category = await categoryService.GetByIdAsync(data.CategoryId, ct);
        var movableItem = await MovableItem.CreateAsync(data.Name, data.Description, data.ImgSrc, category, nameUniquenessChecker.Value, ct);
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

        if (data.ImgSrc != null && movableItem.ImgSrc != data.ImgSrc)
        {
            var fs = fileService.Value;

            if (movableItem.ImgSrc != null)
            {
                await fs.DeleteAsync(movableItem.ImgSrc, ct);
            }

            var (_, newUrl) = await fs.MoveFileFromTmpToUploadsAsync(data.ImgSrc, ct);

            data.ImgSrc = newUrl;
        }
        else if (data.ImgSrc == null && movableItem.ImgSrc != null)
        {
            await fileService.Value.DeleteAsync(movableItem.ImgSrc, ct);
            data.ImgSrc = null;
        }

        await movableItem.UpdateAsync(data.Name, data.Description, data.ImgSrc, category, nameUniquenessChecker.Value, ct);
        await repo.UpdateAsync(movableItem, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(uint id, CancellationToken ct = default)
    {
        await repo.DeleteAsync(id, ct);
        await context.SaveChangesAsync(ct);
    }
}