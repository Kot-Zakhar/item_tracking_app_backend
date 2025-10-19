using ItTrAp.QueryService.Application.Common;
using ItTrAp.QueryService.Application.Filters;
using ItTrAp.QueryService.Application.Interfaces;
using ItTrAp.QueryService.Application.Responses;
using ItTrAp.QueryService.Domain.Enums;
using ItTrAp.QueryService.Infrastructure.Interfaces.Services;

namespace ItTrAp.QueryService.Infrastructure.Services;

// TODO: this service loads the complete list of core entity before paginating it
// TODO: think of where to filter and paginate: here or in the underlying services?
// TODO: how to solve the problem of inconsistent data when loading from multiple services?

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

    public async Task<PaginatedResponse<MovableInstanceViewModel>> GetMovableInstancesAsync(Guid movableItemId, PaginatedFilteredQuery<MovableInstanceFiltersDto> query, CancellationToken cancellationToken = default)
    {
        // query is build out of 4 components:
        // 1. List of MovableInstanceDto's from InventoryService
        var instances = await inventoryService.GetMovableInstancesByItemIdAsync(movableItemId, cancellationToken);
        // 2. For each MovableInstanceDto get status, locationId and userId from ManagementService
        var instanceStatuses = await managementService.GetInstanceStatusesByItemAsync(movableItemId, cancellationToken);
        // 3. For each MovableInstanceDto get LocationViewModel from LocationService (if assigned)
        var locationsIds = instanceStatuses.Where(i => i.LocationId.HasValue).Select(i => i.LocationId!.Value).Distinct().ToList();
        var locations = await locationService.GetLocationsByIdsAsync(locationsIds, cancellationToken);
        var locationDict = locations.ToDictionary(l => l.Id, l => l);
        // 4. For each MovableInstanceDto get UserViewModel from UserService (if assigned)
        var userIds = instanceStatuses.Where(i => i.UserId.HasValue).Select(i => i.UserId!.Value).Distinct().ToList();
        var users = await userService.GetUsersByIdsAsync(userIds, cancellationToken);
        var userDict = users.ToDictionary(u => u.Id, u => u);

        // combine all these data into MovableInstanceViewModel and return a paginated response
        var instanceDetails = instances.Select(instance =>
        {
            var statusInfo = instanceStatuses.FirstOrDefault(s => s.Id == instance.Id);

            locationDict.TryGetValue(statusInfo?.LocationId ?? 0, out var location);
            userDict.TryGetValue(statusInfo?.UserId ?? 0, out var user);

            return new MovableInstanceViewModel
            {
                Id = instance.Id,
                CreatedAt = instance.CreatedAt,
                Status = statusInfo?.Status ?? MovableInstanceStatus.Available, // TODO: default to unavailable
                Location = location != null ? new LocationViewModel
                {
                    Id = location.Id,
                    Name = location.Name,
                    Floor = location.Floor,
                    Department = location.Department,
                    CreatedAt = location.CreatedAt
                } : null,
                User = user != null ? new UserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Phone = user.Phone,
                    Avatar = user.Avatar
                } : null
            };
        }).ToList();
        
        return new PaginatedResponse<MovableInstanceViewModel>
        {
            TotalAmount = instances.Count,
            Payload = instanceDetails
        };
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

    public async Task<PaginatedResponse<UserWithDetailsViewModel>> GetUsersWithDetailsAsync(PaginatedFilteredQuery<UserFiltersDto> query, CancellationToken cancellationToken = default)
    {
        var users = await userService.GetUsersAsync(cancellationToken);

        var pagedUsers = users.Skip(query.PageSize * query.PageIndex).Take(query.PageSize).ToList();
        var userIds = pagedUsers.Select(u => u.Id).ToList();

        var itemAmounts = await managementService.GetItemAmountsByUserIdsAsync(userIds, cancellationToken);
        var userDetails = pagedUsers.Zip(itemAmounts, (user, amt) => new UserWithDetailsViewModel
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Phone = user.Phone,
            Avatar = user.Avatar,
            ItemsAmount = amt
        });

        if (query.Filters?.HasInstances == true)
        {
            userDetails = userDetails.Where(ud => ud.ItemsAmount > 0);
        }

        if (query.Filters?.Search is not null)
        {
            var searchLower = query.Filters.Search.ToLower();
            userDetails = userDetails.Where(ud =>
                ud.FirstName.ToLower().Contains(searchLower) ||
                ud.LastName.ToLower().Contains(searchLower) ||
                ud.Email.ToLower().Contains(searchLower) ||
                ud.Phone.Contains(searchLower)
            );
        }

        return new PaginatedResponse<UserWithDetailsViewModel>
        {
            TotalAmount = users.Count,
            Payload = userDetails.ToList()
        };
    }
}