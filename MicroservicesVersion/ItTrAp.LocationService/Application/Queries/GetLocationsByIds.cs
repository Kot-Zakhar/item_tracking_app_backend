using ItTrAp.LocationService.Application.DTOs;
using ItTrAp.LocationService.Application.Interfaces;
using MediatR;

namespace ItTrAp.LocationService.Application.Queries;

public record GetLocationsByIdsQuery(IList<uint> Ids) : IRequest<IList<LocationDto>>;

public class GetLocationsByIdsQueryHandler : IRequestHandler<GetLocationsByIdsQuery, IList<LocationDto>>
{
    private readonly ILocationReadRepository _locationReadRepository;

    public GetLocationsByIdsQueryHandler(ILocationReadRepository locationReadRepository)
    {
        _locationReadRepository = locationReadRepository;
    }

    public async Task<IList<LocationDto>> Handle(GetLocationsByIdsQuery request, CancellationToken cancellationToken)
    {
        var locations = await _locationReadRepository.GetLocationsByIdsAsync(request.Ids, cancellationToken);
        return locations;
    }
}