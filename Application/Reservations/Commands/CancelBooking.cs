using Application.Reservations.Interfaces;
using FluentValidation;
using MediatR;

namespace Application.Reservations.Commands;

public record CancelBookingCommand(uint IssuerId, uint InstanceId) : IRequest;

public class CancelBookingCommandValidator : AbstractValidator<CancelBookingCommand>
{
    public CancelBookingCommandValidator()
    {
        RuleFor(x => (int)x.IssuerId).GreaterThan(0);
        RuleFor(x => (int)x.InstanceId).GreaterThan(0);
    }
}

public class CancelBookingCommandHandler(IReservationService reservationsService) : IRequestHandler<CancelBookingCommand>
{
    public async Task Handle(CancelBookingCommand request, CancellationToken cancellationToken)
    {
        // Assuming the service has a method to cancel a booking
        await reservationsService.CancelBookingAsync(request.IssuerId, request.InstanceId, cancellationToken);
    }
}