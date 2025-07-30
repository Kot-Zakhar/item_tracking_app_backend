using ItTrAp.ManagementService.Application.Interfaces.Services;
using FluentValidation;
using MediatR;

namespace ItTrAp.ManagementService.Application.Commands.Reservations;

public record BookAnyInstanceInLocationCommand(uint IssuerId, uint UserId, Guid ItemId, uint LocationId) : IRequest<uint>;

public class BookAnyInstanceInLocationValidator : AbstractValidator<BookAnyInstanceInLocationCommand>
{
    public BookAnyInstanceInLocationValidator()
    {
        RuleFor(x => (int)x.IssuerId).GreaterThan(0);
        RuleFor(x => (int)x.UserId).GreaterThan(0);
        RuleFor(x => x.ItemId).NotEmpty();
        RuleFor(x => (int)x.LocationId).GreaterThan(0);
    }
}

public class BookAnyInstanceInLocationHandler(IReservationService reservationsService) : IRequestHandler<BookAnyInstanceInLocationCommand, uint>
{
    public async Task<uint> Handle(BookAnyInstanceInLocationCommand request, CancellationToken cancellationToken)
    {
        return await reservationsService.BookAnyInstanceInLocationAsync(
            request.IssuerId,
            request.UserId,
            request.ItemId,
            request.LocationId,
            cancellationToken);
    }
}