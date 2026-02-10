using ItTrAp.LocationService.Application.Interfaces;
using ItTrAp.LocationService.Application.DTOs;
using MediatR;

namespace ItTrAp.LocationService.Application.Queries;

public record GetLocationByIdQuery(uint Id) : IRequest<LocationDto>;

public class GetLocationByIdHandler(ILocationReadRepository repo) : IRequestHandler<GetLocationByIdQuery, LocationDto?>
{
    public Task<LocationDto?> Handle(GetLocationByIdQuery request, CancellationToken cancellationToken)
    {
        return repo.GetByIdAsync(request.Id, cancellationToken);
    }
}