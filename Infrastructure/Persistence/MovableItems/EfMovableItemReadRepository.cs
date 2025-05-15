using Application.MovableItems.ViewModels;
using Application.MovableItems.Queries;
using Application.Common.ViewModels;
using Microsoft.EntityFrameworkCore;
using Domain.MovableItems;

namespace Infrastructure.Persistence.MovableItems;

// TODO: Use AutoMapper to map joined entities
// TODO: Fix Avatar property in UserViewModel
// TODO: Apply filters to the query

public class EfMovableItemReadRepository : IMovableItemReadRepository
{
    private readonly AppDbContext _dbContext;

    public EfMovableItemReadRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<MovableItemWithDetailsViewModel>> GetAllFilteredWithDetailsAsync(
        MovableInstanceStatus? status,
        uint? categoryId,
        bool? excludeItemsOfChildCategories,
        uint? locationId,
        string? search,
        List<uint>? userIds,
        CancellationToken ct = default
    ) => await _dbContext.MovableItems
            .AsNoTracking()
            .Select(item => new MovableItemWithDetailsViewModel
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Category = new CategoryViewModel
                {
                    Id = item.Category.Id,
                    Name = item.Category.Name,
                    Icon = item.Category.Icon
                },
                Visibility = item.Visibility,
                CreatedAt = item.CreatedAt,
                ImgSrc = item.ImgSrc,
                TotalAmount = item.Instances.Count(),
                BookedBy = item.Instances
                    .Where(instance => instance.Status == MovableInstanceStatus.Booked)
                    .Where(instance => instance.User != null)
                    .Select(instance => new UserViewModel
                    {
                        Id = instance.User!.Id,
                        FirstName = instance.User.FirstName,
                        LastName = instance.User.LastName,
                        Phone = instance.User.Phone,
                        Email = instance.User.Email,
                        // Avatar = instance.User.Avatar
                    })
                    .ToList(),
                TakenBy = item.Instances
                    .Where(instance => instance.Status == MovableInstanceStatus.Taken)
                    .Where(instance => instance.User != null)
                    .Select(instance => new UserViewModel
                    {
                        Id = instance.User!.Id,
                        FirstName = instance.User.FirstName,
                        LastName = instance.User.LastName,
                        Phone = instance.User.Phone,
                        Email = instance.User.Email,
                        // Avatar = instance.User.Avatar
                    })
                    .ToList(),
            })
            .ToListAsync(ct);

    public async Task<List<MovableItemWithAmountsByStatusViewModel>> GetAllWithAmountPerStatusAsync(
        MovableInstanceStatus? status,
        uint? categoryId,
        bool? excludeItemsOfChildCategories,
        uint? locationId,
        string? search,
        List<uint>? userIds,
        CancellationToken ct = default
    ) => await _dbContext.MovableItems
            .AsNoTracking()
            .Select(item => new MovableItemWithAmountsByStatusViewModel
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Category = new CategoryViewModel
                {
                    Id = item.Category.Id,
                    Name = item.Category.Name,
                    Icon = item.Category.Icon
                },
                Visibility = item.Visibility,
                CreatedAt = item.CreatedAt,
                ImgSrc = item.ImgSrc,
                AmountsByStatus = item.Instances
                    .GroupBy(instance => instance.Status)
                    .Select(group => new
                    {
                        Status = group.Key,
                        Amount = group.Count()
                    })
                    .ToDictionary(group => group.Status, group => group.Amount),
            })
            .ToListAsync(ct);

    public async Task<MovableItemViewModel?> GetByIdAsync(uint id) => await _dbContext.MovableItems
                .AsNoTracking()
                .Where(item => item.Id == id)
                .Select(item => new MovableItemViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Category = new CategoryViewModel
                    {
                        Id = item.Category.Id,
                        Name = item.Category.Name,
                        Icon = item.Category.Icon
                    },
                    Visibility = item.Visibility,
                    CreatedAt = item.CreatedAt,
                    ImgSrc = item.ImgSrc,
                })
                .FirstOrDefaultAsync();
}