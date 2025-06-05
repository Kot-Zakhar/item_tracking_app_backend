using Application.Categories.DTOs;
using Application.Categories.Interfaces;
using Application.Common.DTOs;
using Domain.MovableItems;
using Domain.MovableItems.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EFPersistence.Categories;

public class EFCategoryReadRepository(AppDbContext appDbContext) : ICategoryReadRepository, ICategoryUniquenessChecker
{
    public async Task<List<CategoryWithDetailsDto>> GetCategoryTreeAsync(CancellationToken ct = default)
    {
        var categories = await appDbContext.Categories
            .Where(c => c.Parent == null)
            .ToListAsync(ct);

        var amounts = await appDbContext.MovableItems
            .AsNoTracking()
            .GroupBy(i => i.Category.Id)
            .Select(g => new { g.Key, Amount = g.Count() })
            .ToDictionaryAsync(x => x.Key, x => (uint)x.Amount, ct);

        return categories.Select(c => BuildCategoryTreeWithDetails(c, amounts, downwards: true, ct)).ToList();
    }


    public async Task<CategoryWithDetailsDto?> GetCategoryTreeFromNode(uint id, CancellationToken ct = default)
    {
        var category = await appDbContext.Categories
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync(ct);

        if (category is null)
            return null;

        var amounts = await appDbContext.MovableItems
            .AsNoTracking()
            .GroupBy(i => i.Category.Id)
            .Select(g => new { g.Key, Amount = g.Count() })
            .ToDictionaryAsync(x => x.Key, x => (uint)x.Amount, ct);

        return BuildCategoryTreeWithDetails(category, amounts, downwards: null, ct); ;
    }

    public Task<bool> IsUniqueAsync(string name, CancellationToken ct = default)
        => appDbContext.Categories.AllAsync(x => x.Name != name, ct);


    private static CategoryWithDetailsDto BuildCategoryTreeWithDetails(Category category, Dictionary<uint, uint> amounts, bool? downwards = null, CancellationToken ct = default)
    {
        var categoryViewModel = new CategoryWithDetailsDto
        {
            Category = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Icon = category.Icon,
            },
            ItemsAmount = amounts.TryGetValue(category.Id, out var amount) ? amount : 0,
        };

        if (downwards != false)
        {
            categoryViewModel.Children = category.Children
                .Select(c => BuildCategoryTreeWithDetails(c, amounts, true, ct))
                .ToList();
        }

        if (downwards != true && category.Parent != null)
        {
            categoryViewModel.Parent = BuildCategoryTreeWithDetails(category.Parent, amounts, false, ct);
        }

        return categoryViewModel;
    }
}
