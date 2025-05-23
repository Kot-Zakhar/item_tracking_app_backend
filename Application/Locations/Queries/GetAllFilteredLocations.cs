using Application.Locations.Dtos;
using Application.Locations.Interfaces;
using Application.Locations.ViewModels;
using MediatR;

namespace Application.Locations.Queries;

public record GetAllFilteredLocationsQuery(LocationFiltersDto filters) : IRequest<List<LocationWithDetailsViewModel>>;

public class GetAllFilteredLocationsHandler(ILocationReadRepository repo) : IRequestHandler<GetAllFilteredLocationsQuery, List<LocationWithDetailsViewModel>>
{
    public async Task<List<LocationWithDetailsViewModel>> Handle(GetAllFilteredLocationsQuery request, CancellationToken cancellationToken)
    {
        return await repo.GetAllFilteredAsync(request.filters, cancellationToken);
    }
}
