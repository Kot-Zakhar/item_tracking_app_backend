using ItTrAp.InventoryService.Interfaces.Services;
using ItTrAp.InventoryService.Interfaces;
using ItTrAp.InventoryService.Models;
using ItTrAp.InventoryService.Persistence;
using ItTrAp.InventoryService.DTOs.MovableItems;
using ItTrAp.InventoryService.Interfaces.Repositories;
using ItTrAp.InventoryService.Interfaces.Persistence.Repositories;
using ItTrAp.InventoryService.Mappers;

namespace ItTrAp.InventoryService.Services;

public class MovableItemService(
    IMovableItemRepository itemRepository,
    ICategoryRepository categoryRepository,
    Lazy<IMovableItemUniquenessChecker> nameUniquenessChecker,
    Lazy<IFileService> fileService,
    IUnitOfWork unitOfWork) : IMovableItemService
{
    public async Task<Guid> CreateAsync(CreateMovableItemDto data, CancellationToken ct = default)
    {
        var category = await categoryRepository.GetByIdAsync(data.CategoryId, ct);
        if (category == null)
            throw new ArgumentException($"Category with ID {data.CategoryId} not found.");

        var movableItem = await MovableItem.CreateAsync(data, category, nameUniquenessChecker.Value, ct);
        movableItem = await itemRepository.CreateAsync(movableItem, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return movableItem.Id;
    }

    public async Task<MovableItemWithCategoryDto?> GetByIdAsync(Guid itemId, CancellationToken ct = default)
    {
        var itemDto = await itemRepository.GetByIdAsync(itemId, ct);
        if (itemDto == null)
            return null;

        var category = await categoryRepository.GetByIdAsync(itemDto.CategoryId, ct);
        if (category == null)
            throw new ArgumentException($"Category with ID {itemDto.CategoryId} not found.");

        return itemDto.ToDtoWithCategory(category);
    }

    public async Task UpdateAsync(Guid id, UpdateMovableItemDto data, CancellationToken ct = default)
    {
        var movableItem = await itemRepository.GetByIdAsync(id, ct);
        if (movableItem == null)
            throw new ArgumentException($"MovableItem with ID {id} not found.");

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

        await movableItem.UpdateAsync(data, nameUniquenessChecker.Value, ct);
        await itemRepository.UpdateAsync(movableItem, ct);
        await unitOfWork.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        await itemRepository.DeleteAsync(id, ct);
        await unitOfWork.SaveChangesAsync(ct);
    }
}