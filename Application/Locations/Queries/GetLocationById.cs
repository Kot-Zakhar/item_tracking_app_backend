using Application.Common.ViewModels;
using Application.Locations.Interfaces;
using MediatR;

namespace Application.Locations.Queries;

public record GetLocationByIdQuery(uint Id) : IRequest<LocationViewModel>;

public class GetLocationByIdHandler(ILocationReadRepository repo) : IRequestHandler<GetLocationByIdQuery, LocationViewModel?>
{
    public Task<LocationViewModel?> Handle(GetLocationByIdQuery request, CancellationToken cancellationToken)
    {
        return repo.GetByIdAsync(request.Id, cancellationToken);
    }
}