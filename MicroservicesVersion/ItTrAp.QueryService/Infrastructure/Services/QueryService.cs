using ItTrAp.QueryService.Application.Common;
using ItTrAp.QueryService.Application.Filters;
using ItTrAp.QueryService.Application.Interfaces;
using ItTrAp.QueryService.Application.Responses;
using ItTrAp.QueryService.Domain.Enums;
using ItTrAp.QueryService.Infrastructure.Interfaces.Services;

namespace ItTrAp.QueryService.Infrastructure.Services;

// TODO: this service loads the complete list of core entity before paginating it

public class QueryService(
    // ILogger<QueryService> logger,
    ILocationService locationService,
    IManagementService managementService,
    IInventoryService inventoryService,
    IUserService userService) : IQueryService
{
    public async Task<PaginatedResponse<LocationWithDetailsViewModel>> GetLocationsWithDetailsAsync(PaginatedFilteredQuery<LocationFiltersDto> query, CancellationToken cancellationToken = default)
    {
        var locations = await locationService.GetLocationsAsync(query.Filters, cancellationToken);

        var pagedLocations = locations.Skip(query.PageSize * query.PageIndex).Take(query.PageSize).ToList();

        var locationIds = pagedLocations.Select(l => l.Id).ToList();

        var instanceAmounts = await managementService.GetInstanceAmountsInLocationsAsync(locationIds, cancellationToken);

        var locationDetails = pagedLocations.Zip(instanceAmounts, (loc, amt) => new LocationWithDetailsViewModel
        {
            Id = loc.Id,
            Name = loc.Name,
            Floor = loc.Floor,
            Department = loc.Department,
            CreatedAt = loc.CreatedAt,
            InstancesAmount = amt
        }).ToList();

        var payload = locationDetails.Skip(query.PageSize * query.PageIndex).Take(query.PageSize).ToList();

        return new PaginatedResponse<LocationWithDetailsViewModel>
        {
            TotalAmount = locations.Count,
            Payload = payload
        };
    }

    public Task<PaginatedResponse<MovableInstanceViewModel>> GetMovableInstancesAsync(uint movableItemId, PaginatedFilteredQuery<MovableInstanceFiltersDto> query, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<PaginatedResponse<MovableItemWithDetailsViewModel>> GetMovableItemsWithDetailsAsync(PaginatedFilteredQuery<MovableItemFiltersDto> query, CancellationToken cancellationToken = default)
    {
        // query is build out of 4 components:
        // 1. List of MovableItemDto's from InventoryService
        var items = await inventoryService.GetMovableItemsAsync(query.Filters?.CategoryIds, query.Filters?.Search, cancellationToken);

        var paginatedItems = items.Skip(query.PageSize * query.PageIndex).Take(query.PageSize).ToList();

        var itemIds = paginatedItems.Select(i => i.Id).ToList();

        // 2. For each MovableItemDto get totalAmount of instances from InventoryService
        var instanceAmounts = await inventoryService.GetInstanceAmountsByItemIdsAsync(itemIds, cancellationToken);
        var instanceAmountByItemDict = itemIds.Zip(instanceAmounts, (id, amt) => new { id, amt }).ToDictionary(x => x.id, x => x.amt);

        // 3. For each MovableItemDto get a dictionary of users by statuses from ManagementService
        var userStatusesByItem = await managementService.GetUserStatusesForItemsAsync(itemIds, cancellationToken);

        var userIds = userStatusesByItem.SelectMany(us => us.Value).Select(us => us.Value).Distinct().ToList();

        // 4. For each User get UserDto from UserService
        var users = await userService.GetUsersByIdsAsync(userIds, cancellationToken);
        var userDict = users.ToDictionary(u => u.Id, u => u);

        // combine all these data into MovableItemWithDetailsViewModel and return a paginated response
        var itemDetails = paginatedItems.Select(item =>
        {
            var userIdStatus = userStatusesByItem.ContainsKey(item.Id)
                ? userStatusesByItem[item.Id]
                : new List<KeyValuePair<MovableInstanceStatus, uint>>();
            var usersForItem = userIdStatus.Select(us => new { us.Key, User = userDict[us.Value] }).GroupBy(x => x.Key, x => x.User);

            var totalAmount = instanceAmountByItemDict[item.Id];

            return new MovableItemWithDetailsViewModel
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Category = item.Category,
                CreatedAt = item.CreatedAt,
                TotalAmount = totalAmount,
                UsersByStatus = usersForItem.ToDictionary(g => g.Key, g => g.ToList())
            };
        }).ToList();

        return new PaginatedResponse<MovableItemWithDetailsViewModel>
        {
            TotalAmount = items.Count,
            Payload = itemDetails
        };
    }

    public Task<PaginatedResponse<UserWithDetailsViewModel>> GetUsersAsync(PaginatedFilteredQuery<UserFiltersDto> query, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}