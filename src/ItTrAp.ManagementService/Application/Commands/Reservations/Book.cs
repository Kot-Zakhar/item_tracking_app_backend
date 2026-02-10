using ItTrAp.ManagementService.Application.Interfaces.Services;
using FluentValidation;
using MediatR;

namespace ItTrAp.ManagementService.Application.Commands.Reservations;

public record BookCommand(uint IssuerId, uint BookerId, uint InstanceId) : IRequest;

public class BookCommandValidator : AbstractValidator<BookCommand>
{
    public BookCommandValidator()
    {
        RuleFor(x => (int)x.InstanceId).NotEqual(0).WithMessage("Instance ID must not be zero.");
        RuleFor(x => (int)x.IssuerId).NotEqual(0).WithMessage("Issuer ID must not be zero.");
        RuleFor(x => (int)x.BookerId).NotEqual(0).WithMessage("Booker ID must not be zero.");
    }
}

public class BookCommandHandler(IReservationService reservationsService) : IRequestHandler<BookCommand>
{
    public async Task Handle(BookCommand request, CancellationToken cancellationToken)
    {
        await reservationsService.BookAsync(request.IssuerId, request.BookerId, request.InstanceId, cancellationToken);
    }
}