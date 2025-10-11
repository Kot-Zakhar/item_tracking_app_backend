using ItTrAp.QueryService.Application.Common;
using ItTrAp.QueryService.Application.Filters;
using ItTrAp.QueryService.Application.Interfaces;
using ItTrAp.QueryService.Application.Responses;
using Microsoft.Extensions.Options;
using ItTrAp.QueryService.Infrastructure.Interfaces.Services;
using ItTrAp.QueryService.Infrastructure.Interfaces.Service;

namespace ItTrAp.QueryService.Infrastructure.Services;

public class QueryService(
    ILogger<QueryService> logger,
    IOptions<GlobalConfig> config,
    ILocationService locationService,
    IManagementService managementService) : IQueryService
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

    public Task<PaginatedResponse<MovableItemWithDetailsViewModel>> GetMovableItemsAsync(PaginatedFilteredQuery<MovableItemFiltersDto> query, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
        // query is build out of 4 components:
        // 1. List of MovableItemDto's from InventoryService
        // 2. For each MovableItemDto get totalAmount of instances from InventoryService
        // 3. For each MovableItemDto get a dictionary of users by statuses from ManagementService
        // 4. For each User get UserDto from UserService
        // combine all these data into MovableItemWithDetailsViewModel and return a paginated response
    }

    public Task<PaginatedResponse<UserWithDetailsViewModel>> GetUsersAsync(PaginatedFilteredQuery<UserFiltersDto> query, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}