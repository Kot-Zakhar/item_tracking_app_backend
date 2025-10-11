using ItTrAp.QueryService.Application.Filters;
using ItTrAp.QueryService.Application.Interfaces;
using ItTrAp.QueryService.Application.Common;
using ItTrAp.QueryService.Application.Responses;
using MediatR;

namespace ItTrAp.QueryService.Application.Queries;

public record GetFilteredLocationsQuery() : PaginatedFilteredQuery<LocationFiltersDto>, IRequest<PaginatedResponse<LocationWithDetailsViewModel>>;

public class GetFilteredLocationsQueryHandler(IQueryService queryService) : IRequestHandler<GetFilteredLocationsQuery, PaginatedResponse<LocationWithDetailsViewModel>>
{
    public async Task<PaginatedResponse<LocationWithDetailsViewModel>> Handle(GetFilteredLocationsQuery request, CancellationToken cancellationToken)
    {
        return await queryService.GetLocationsWithDetailsAsync(request, cancellationToken);
    }
}