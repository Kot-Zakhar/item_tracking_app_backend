using Grpc.Core;
using ItTrAp.LocationService.Application.DTOs;
using ItTrAp.LocationService.Application.Interfaces;
using ItTrAp.LocationService.Application.Queries;
using ItTrAp.LocationService.Infrastructure.Protos;
using MediatR;

namespace ItTrAp.LocationService.Infrastructure.Servers;

public class GrpcServer : LocationServer.LocationServerBase
{
    private readonly ILogger<GrpcServer> _logger;
    private readonly ILocationReadRepository _locationReadRepository;
    private readonly IMediator _mediator;

    public GrpcServer(ILogger<GrpcServer> logger, ILocationReadRepository locationReadRepository, IMediator mediator)
    {
        _logger = logger;
        _locationReadRepository = locationReadRepository;
        _mediator = mediator;
    }

    public override async Task<GetLocationsResponse> GetLocations(GetLocationsRequest request, ServerCallContext context)
    {
        _logger.LogDebug("Received GetLocations request with filter: {@Filter}", request.Filter);

        var filter = new LocationFiltersDto
        {
            Search = request.Filter?.Search,
            Floor = (sbyte?)request.Filter?.Floor
        };

        var query = new GetAllFilteredLocationsQuery(filter);

        var locations = await _mediator.Send(query, context.CancellationToken);

        var response = new GetLocationsResponse();
        response.Locations.AddRange(locations.Select(MapToProto));
        return response;
    }

    private Location MapToProto(LocationDto dto)
    {
        return new Location
        {
            Id = dto.Id,
            Floor = (int)dto.Floor,
            Name = dto.Name,
            Department = dto.Department,
            CreatedAt = (ulong)((DateTimeOffset)dto.CreatedAt).ToUnixTimeSeconds(),
        };
    }
}