using MediatR;
using ItTrAp.InventoryService.Infrastructure.Protos;
using Grpc.Core;

namespace ItTrAp.InventoryService.Infrastructure.Servers;

public class GrpcServer : InventoryServer.InventoryServerBase
{
    private readonly ILogger<GrpcServer> _logger;
    private readonly IMediator _mediator;

    public GrpcServer(ILogger<GrpcServer> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public override async Task<GetMovableItemsResponse> GetMovableItems(GetMovableItemsRequest request, ServerCallContext context)
    {
        _logger.LogDebug("Received gRPC request for all movable items");

        var items = await _mediator.Send(new Application.Queries.MovableItems.GetMovableItemsQuery(), context.CancellationToken);

        var response = new GetMovableItemsResponse();
        response.Items.AddRange(items.Select(MapToProto));
        return response;
    }

    public override async Task<GetMovableInstancesByItemIdResponse> GetMovableInstancesByItemId(GetMovableInstancesByItemIdRequest request, ServerCallContext context)
    {
        var itemId = Guid.Parse(request.ItemId);
        _logger.LogDebug("Received gRPC request for movable instances by item ID: {ItemId}", itemId);

        var instances = await _mediator.Send(new Application.Queries.MovableInstances.GetMovableInstancesByItemIdQuery(itemId), context.CancellationToken);

        var response = new GetMovableInstancesByItemIdResponse();
        response.Instances.AddRange(instances.Select(i => new MovableInstance
        {
            Id = i.Id,
            MovableItemId = itemId.ToString(),
            CreatedAt = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(i.CreatedAt.ToUniversalTime()),
        }));
        return response;
    }

    public override async Task<GetInstanceAmountsByItemIdsResponse> GetInstanceAmountsByItemIds(GetInstanceAmountsByItemIdsRequest request, ServerCallContext context)
    {
        var ids = request.Ids.Select(Guid.Parse).ToList();
        _logger.LogDebug("Received gRPC request for instance amounts by item IDs: {Ids}", string.Join(", ", ids));

        var amounts = await _mediator.Send(new Application.Queries.MovableItems.GetInstanceAmountsByItemIdsQuery(ids), context.CancellationToken);

        var response = new GetInstanceAmountsByItemIdsResponse();
        response.Amounts.AddRange(amounts.Select(a => (uint)a));
        return response;
    }

    private static MovableItem MapToProto(Application.DTOs.MovableItems.MovableItemWithCategoryDto item)
    {
        return new MovableItem
        {
            Id = item.Id.ToString(),
            Name = item.Name,
            Description = item.Description,
            CategoryId = item.CategoryId,
            Category = new Category
            {
                Id = item.Category.Id,
                Name = item.Category.Name,
                Icon = item.Category.Icon,
            },
            ImgSrc = item.ImgSrc,
            CreatedAt = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(item.CreatedAt.ToUniversalTime()),
        };
    }
}