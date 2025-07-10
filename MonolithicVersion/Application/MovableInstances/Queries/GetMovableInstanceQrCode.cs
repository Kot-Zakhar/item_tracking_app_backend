using Application.MovableInstances.Interfaces;
using MediatR;

namespace Application.MovableInstances.Queries;

public record GetMovableInstanceQrCodeQuery(uint ItemId, uint InstanceId) : IRequest<byte[]>;

public class GetMovableInstanceQrCodeQueryHandler(IMovableInstanceService movableInstanceService) : IRequestHandler<GetMovableInstanceQrCodeQuery, byte[]>
{
    public Task<byte[]> Handle(GetMovableInstanceQrCodeQuery request, CancellationToken cancellationToken)
    {
        return movableInstanceService.GetQrCodeAsync(request.ItemId, request.InstanceId, cancellationToken);
    }
}