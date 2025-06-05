using Application.Reservations.Interfaces;
using FluentValidation;
using MediatR;

namespace Application.Reservations.Commands;

public record CancelBookingCommand(uint UserId, uint InstanceId) : IRequest;

public class CancelBookingCommandValidator : AbstractValidator<CancelBookingCommand>
{
    public CancelBookingCommandValidator()
    {
        RuleFor(x => (int)x.UserId).GreaterThan(0);
        RuleFor(x => (int)x.InstanceId).GreaterThan(0);
    }
}

public class CancelBookingCommandHandler(IReservationService reservationsService) : IRequestHandler<CancelBookingCommand>
{
    public async Task Handle(CancelBookingCommand request, CancellationToken cancellationToken)
    {
        // Assuming the service has a method to cancel a booking
        await reservationsService.CancelBookingAsync(request.UserId, request.InstanceId, cancellationToken);
    }
}