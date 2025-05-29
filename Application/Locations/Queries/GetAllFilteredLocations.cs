using Application.Locations.DTOs;
using Application.Locations.Interfaces;
using MediatR;

namespace Application.Locations.Queries;

public record GetAllFilteredLocationsQuery(LocationFiltersDto filters) : IRequest<List<LocationWithDetailsDto>>;

public class GetAllFilteredLocationsHandler(ILocationReadRepository repo) : IRequestHandler<GetAllFilteredLocationsQuery, List<LocationWithDetailsDto>>
{
    public async Task<List<LocationWithDetailsDto>> Handle(GetAllFilteredLocationsQuery request, CancellationToken cancellationToken)
    {
        return await repo.GetAllFilteredAsync(request.filters, cancellationToken);
    }
}
