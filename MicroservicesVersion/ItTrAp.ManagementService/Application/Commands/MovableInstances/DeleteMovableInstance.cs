using ItTrAp.ManagementService.Application.Interfaces.Services;
using MediatR;

namespace ItTrAp.ManagementService.Application.Commands.MovableInstances;

public record DeleteMovableInstanceCommand(Guid ItemId, uint Id, uint IssuerId) : IRequest;

public class DeleteMovableInstanceCommandHandler(IMovableInstanceService movableInstanceService) : IRequestHandler<DeleteMovableInstanceCommand>
{
    public async Task Handle(DeleteMovableInstanceCommand request, CancellationToken cancellationToken)
    {
        await movableInstanceService.DeleteAsync(request.ItemId, request.Id, request.IssuerId, cancellationToken);
    }
}