using Grpc.Core;
using ItTrAp.ManagementService.Application.Queries.MovableInstances;
using ItTrAp.ManagementService.Infrastructure.Protos;
using MediatR;

namespace ItTrAp.ManagementService.Infrastructure.Servers;

public class GrpcServer : ManagementServer.ManagementServerBase
{
    private readonly ILogger<GrpcServer> _logger;
    private readonly IMediator _mediator;

    public GrpcServer(ILogger<GrpcServer> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public override async Task<GetInstanceAmountResponse> GetInstanceAmountInLocations(GetInstanceAmountRequest request, ServerCallContext context)
    {
        var locationIds = request.Ids.ToList();
        _logger.LogDebug("Received gRPC request for instance amounts in locations: {LocationIds}", string.Join(", ", locationIds));

        var amounts = await _mediator.Send(new GetInstanceAmountsInLocationsQuery(locationIds), context.CancellationToken);

        var response = new GetInstanceAmountResponse();
        response.Amounts.AddRange(amounts);
        return response;
    }

}