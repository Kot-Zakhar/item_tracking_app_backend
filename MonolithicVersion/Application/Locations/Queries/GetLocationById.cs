using Application.Common.DTOs;
using Application.Locations.Interfaces;
using MediatR;

namespace Application.Locations.Queries;

public record GetLocationByIdQuery(uint Id) : IRequest<LocationDto>;

public class GetLocationByIdHandler(ILocationReadRepository repo) : IRequestHandler<GetLocationByIdQuery, LocationDto?>
{
    public Task<LocationDto?> Handle(GetLocationByIdQuery request, CancellationToken cancellationToken)
    {
        return repo.GetByIdAsync(request.Id, cancellationToken);
    }
}