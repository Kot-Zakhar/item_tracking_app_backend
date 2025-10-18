using ItTrAp.QueryService.Application.Filters;
using ItTrAp.QueryService.Application.Common;
using ItTrAp.QueryService.Application.Responses;

namespace ItTrAp.QueryService.Application.Interfaces;

public interface IQueryService
{
    Task<PaginatedResponse<LocationWithDetailsViewModel>> GetLocationsWithDetailsAsync(PaginatedFilteredQuery<LocationFiltersDto> query, CancellationToken cancellationToken = default);
    Task<PaginatedResponse<MovableInstanceViewModel>> GetMovableInstancesAsync(Guid movableItemId, PaginatedFilteredQuery<MovableInstanceFiltersDto> query, CancellationToken cancellationToken = default);
    Task<PaginatedResponse<MovableItemWithDetailsViewModel>> GetMovableItemsWithDetailsAsync(PaginatedFilteredQuery<MovableItemFiltersDto> query, CancellationToken cancellationToken = default);
    Task<PaginatedResponse<UserWithDetailsViewModel>> GetUsersAsync(PaginatedFilteredQuery<UserFiltersDto> query, CancellationToken cancellationToken = default);
}