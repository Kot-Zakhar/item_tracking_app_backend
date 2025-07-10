using Application.Locations.Interfaces;
using MediatR;

namespace Application.Locations.Queries;

public record GetLocationQrCodeQuery(uint Id) : IRequest<byte[]>;

public class GetLocationQrCodeQueryHandler : IRequestHandler<GetLocationQrCodeQuery, byte[]>
{
    private readonly ILocationService _locationService;

    public GetLocationQrCodeQueryHandler(ILocationService locationService)
    {
        _locationService = locationService;
    }

    public async Task<byte[]> Handle(GetLocationQrCodeQuery request, CancellationToken cancellationToken)
    {
        return await _locationService.GetQrCodeAsync(request.Id, cancellationToken);
    }
}