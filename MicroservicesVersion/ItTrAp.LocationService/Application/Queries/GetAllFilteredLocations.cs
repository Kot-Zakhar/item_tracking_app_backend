using ItTrAp.LocationService.Application.DTOs;
using ItTrAp.LocationService.Application.Interfaces;
using MediatR;

namespace ItTrAp.LocationService.Application.Queries;

public record GetAllFilteredLocationsQuery(LocationFiltersDto filters) : IRequest<IList<LocationDto>>;

public class GetAllFilteredLocationsHandler(ILocationReadRepository repo) : IRequestHandler<GetAllFilteredLocationsQuery, IList<LocationDto>>
{
    public async Task<IList<LocationDto>> Handle(GetAllFilteredLocationsQuery request, CancellationToken cancellationToken)
    {
        return await repo.GetAllFilteredAsync(request.filters, cancellationToken);
    }
}
