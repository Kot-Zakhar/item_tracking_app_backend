using Application.Reservations.Interfaces;
using FluentValidation;
using MediatR;

namespace Application.Reservations.Commands;

public record MoveCommand(uint UserId, uint InstanceId, uint LocationId) : IRequest;

public class MoveCommandValidator : AbstractValidator<MoveCommand>
{
    public MoveCommandValidator()
    {
        RuleFor(x => (int)x.InstanceId).GreaterThan(0).WithMessage("Instance ID must be greater than 0.");
        RuleFor(x => (int)x.LocationId).GreaterThan(0).WithMessage("Location ID must be greater than 0.");
        RuleFor(x => (int)x.UserId).GreaterThan(0).WithMessage("User ID must be greater than 0.");
    }
}

public class MoveCommandHandler(IReservationService reservationService) : IRequestHandler<MoveCommand>
{
    public async Task Handle(MoveCommand request, CancellationToken cancellationToken)
    {
        await reservationService.MoveAsync(request.UserId, request.InstanceId, request.LocationId, cancellationToken);
    }
}