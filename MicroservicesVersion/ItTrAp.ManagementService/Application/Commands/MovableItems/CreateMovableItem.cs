using ItTrAp.ManagementService.Application.Interfaces.Services;
using MediatR;

namespace ItTrAp.ManagementService.Application.Commands.MovableItems;

public record CreateMovableItem(Guid ItemId) : IRequest;

public class CreateMovableItemHandler(IMovableItemService movableItemService) : IRequestHandler<CreateMovableItem>
{
    public async Task Handle(CreateMovableItem request, CancellationToken cancellationToken)
    {
        await movableItemService.CreateAsync(request.ItemId, cancellationToken);
    }
}