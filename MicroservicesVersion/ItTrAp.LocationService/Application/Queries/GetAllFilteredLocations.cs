using ItTrAp.LocationService.Application.DTOs;
using ItTrAp.LocationService.Application.Interfaces;
using MediatR;

namespace ItTrAp.LocationService.Application.Queries;

public record GetAllFilteredLocationsQuery(LocationFiltersDto filters) : IRequest<List<LocationDto>>;

public class GetAllFilteredLocationsHandler(ILocationReadRepository repo) : IRequestHandler<GetAllFilteredLocationsQuery, List<LocationDto>>
{
    public async Task<List<LocationDto>> Handle(GetAllFilteredLocationsQuery request, CancellationToken cancellationToken)
    {
        return await repo.GetAllFilteredAsync(request.filters, cancellationToken);
    }
}
