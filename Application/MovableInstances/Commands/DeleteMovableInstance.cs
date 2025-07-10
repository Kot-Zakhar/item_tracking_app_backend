using Application.MovableInstances.Interfaces;
using MediatR;

namespace Application.MovableInstances.Commands;

public record DeleteMovableInstanceCommand(uint ItemId, uint Id, uint IssuerId) : IRequest;

public class DeleteMovableInstanceCommandHandler(IMovableInstanceService movableInstanceService) : IRequestHandler<DeleteMovableInstanceCommand>
{
    public async Task Handle(DeleteMovableInstanceCommand request, CancellationToken cancellationToken)
    {
        await movableInstanceService.DeleteAsync(request.ItemId, request.Id, request.IssuerId, cancellationToken);
    }
}