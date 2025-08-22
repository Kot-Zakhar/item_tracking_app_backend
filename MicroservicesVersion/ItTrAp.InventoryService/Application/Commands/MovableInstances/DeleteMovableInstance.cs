using ItTrAp.InventoryService.Application.Interfaces.Services;
using MediatR;

namespace ItTrAp.InventoryService.Application.Commands.MovableInstances;

public record DeleteMovableInstanceCommand(Guid ItemId, uint Id) : IRequest;

public class DeleteMovableInstanceCommandHandler(IMovableInstanceService movableInstanceService) : IRequestHandler<DeleteMovableInstanceCommand>
{
    public async Task Handle(DeleteMovableInstanceCommand request, CancellationToken cancellationToken)
    {
        await movableInstanceService.DeleteAsync(request.ItemId, request.Id, cancellationToken);
    }
}