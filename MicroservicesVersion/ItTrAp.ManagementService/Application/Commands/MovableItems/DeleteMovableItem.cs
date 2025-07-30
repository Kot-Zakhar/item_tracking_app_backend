using ItTrAp.ManagementService.Application.Interfaces.Services;
using MediatR;

namespace ItTrAp.ManagementService.Application.Commands.MovableItems;

public record DeleteMovableItem(Guid ItemId) : IRequest;

public class DeleteMovableItemHandler(IMovableItemService movableItemService) : IRequestHandler<DeleteMovableItem>
{
    public async Task Handle(DeleteMovableItem request, CancellationToken cancellationToken)
    {
        await movableItemService.DeleteAsync(request.ItemId, cancellationToken);
    }
}