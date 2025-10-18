using Grpc.Core;
using ItTrAp.ManagementService.Application.Queries.Reservations;
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

    public override async Task<GetInstanceStatusesByItemResponse> GetInstanceStatusesByItem(GetInstanceStatusesByItemRequest request, ServerCallContext context)
    {
        var itemId = Guid.Parse(request.ItemId);
        _logger.LogDebug("Received gRPC request for instance statuses by item: {ItemId}", itemId);

        var statuses = await _mediator.Send(new GetInstanceStatusesByItemQuery(itemId), context.CancellationToken);

        var response = new GetInstanceStatusesByItemResponse();
        response.Statuses.AddRange(statuses.Select(s => new Protos.MovableInstanceStatus
        {
            Id = s.Id,
            ItemId = s.ItemId.ToString(),
            Status = (uint)s.Status,
            UserId = s.UserId,
            LocationId = s.LocationId
        }));
        return response;
    }

    public override async Task<GetUserStatusesForItemsResponse> GetUserStatusesForItems(GetUserStatusesForItemsRequest request, ServerCallContext context)
    {
        var itemIds = request.Ids.Select(Guid.Parse).ToList();
        _logger.LogDebug("Received gRPC request for user statuses for items: {ItemIds}", string.Join(", ", itemIds));

        var statuses = await _mediator.Send(new GetUserStatusesForItemsQuery(itemIds), context.CancellationToken);

        var response = new GetUserStatusesForItemsResponse();
        var userStatusesForItems = statuses.Select(kv =>
        {
            var userStatusesForItem = new UserStatusesForItem
            {
                ItemId = kv.Key.ToString(),
            };

            var userStatuses = kv.Value.Select(us => new UserStatus
            {
                UserId = us.UserId,
                Status = (uint)us.Status
            });

            userStatusesForItem.UserStatuses.AddRange(userStatuses);
            return userStatusesForItem;
        });
        response.UserStatusesForItems.AddRange(userStatusesForItems);
        return response;
    }
}