using Application.Reservations.Interfaces;
using Domain.MovableItems;
using FluentValidation;
using MediatR;

namespace Application.Reservations.Commands;

public record ChangeInstanceStatusCommand(uint InstanceId, MovableInstanceStatus Status, uint? LocationId, uint UserId) : IRequest;

public class ChangeInstanceStatusCommandValidator : AbstractValidator<ChangeInstanceStatusCommand>
{
    public ChangeInstanceStatusCommandValidator()
    {
        RuleFor(x => (int)x.InstanceId).GreaterThan(0);
        RuleFor(x => x.Status).IsInEnum();
        RuleFor(x => (int?)x.LocationId).GreaterThan(0);
        RuleFor(x => (int)x.UserId).GreaterThan(0);
    }
}
// TODO: this command should not exist
// It's an old version of API
public class ChangeInstanceStatusCommandHandler(IReservationService reservationService) : IRequestHandler<ChangeInstanceStatusCommand>
{
    public async Task Handle(ChangeInstanceStatusCommand request, CancellationToken cancellationToken)
    {
        if (request.Status == MovableInstanceStatus.Taken)
        {
            await reservationService.TakeAsync(request.InstanceId, request.UserId, cancellationToken);
        }
        else if (request.Status == MovableInstanceStatus.Booked)
        {
            await reservationService.BookAsync(request.UserId, request.InstanceId, cancellationToken);
        }
        else if (request.Status == MovableInstanceStatus.Available)
        {
            // TODO: In old API this method is used with manager's ID, therefore userId is not passed
            await reservationService.MoveOrReleaseAsync(request.UserId, request.InstanceId, request.LocationId, cancellationToken);
        }
}
}