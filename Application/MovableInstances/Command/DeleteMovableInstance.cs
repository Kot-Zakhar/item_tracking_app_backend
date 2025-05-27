using Application.MovableInstances.Interfaces;
using MediatR;

namespace Application.MovableInstances.Command;

public record DeleteMovableInstanceCommand(uint ItemId, uint Id) : IRequest;

public class DeleteMovableInstanceCommandHandler(IMovableInstanceService movableInstanceService) : IRequestHandler<DeleteMovableInstanceCommand>
{
    public async Task Handle(DeleteMovableInstanceCommand request, CancellationToken cancellationToken)
    {
        await movableInstanceService.DeleteAsync(request.ItemId, request.Id, cancellationToken);
    }
}