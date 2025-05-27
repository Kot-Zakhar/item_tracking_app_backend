using Application.Categories.Interfaces;
using Application.MovableItems.Dtos;
using Application.MovableItems.Interfaces;
using Domain.Categories;
using Domain.Common.Interfaces;
using Domain.MovableItems;
using Domain.MovableItems.Interfaces;
using Infrastructure.Interfaces.MovableItems;
using Infrastructure.Persistence;

namespace Infrastructure.Services.MovableItems;

public class MovableItemService : IMovableItemService
{
  private readonly ICategoryService _categoryService;
  private readonly IMovableItemRepository _repo;
  private readonly Lazy<IMovableItemUniquenessChecker> _nameUniquenessChecker;
  private readonly AppDbContext _context;

    public MovableItemService(
        ICategoryService categoryService,
        IMovableItemRepository repo,
        Lazy<IMovableItemUniquenessChecker> nameUniquenessChecker,
        AppDbContext context)
    {
        _categoryService = categoryService;
        _repo = repo;
        _nameUniquenessChecker = nameUniquenessChecker;
        _context = context;
    }

    public async Task<uint> CreateAsync(CreateMovableItemDto data, CancellationToken ct = default)
    {
        var category = await _categoryService.GetByIdAsync(data.CategoryId, ct);
        var movableItem = await MovableItem.CreateAsync(data.Name, data.Description, data.ImageUrl, category, _nameUniquenessChecker.Value, ct);
        movableItem = await _repo.CreateAsync(movableItem, ct);
        await _context.SaveChangesAsync(ct);
        return movableItem.Id;
    }

    public async Task<MovableItem?> GetByIdAsync(uint itemId, CancellationToken ct = default)
    {
        return await _repo.GetByIdAsync(itemId, ct);
    }

    public async Task UpdateAsync(uint id, UpdateMovableItemDto data, CancellationToken ct = default)
    {
        var movableItem = await _repo.GetByIdAsync(id, ct);
        if (movableItem == null)
            throw new ArgumentException($"MovableItem with ID {id} not found.");

        Category? category = null;
        if (data.CategoryId.HasValue)
        {
            category = await _categoryService.GetByIdAsync(data.CategoryId.Value, ct);
        }

        await movableItem.UpdateAsync(data.Name, data.Description, data.ImageUrl, category, _nameUniquenessChecker.Value, ct);
        await _repo.UpdateAsync(movableItem, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(uint id, CancellationToken ct = default)
    {
        await _repo.DeleteAsync(id, ct);
        await _context.SaveChangesAsync(ct);
    }
}