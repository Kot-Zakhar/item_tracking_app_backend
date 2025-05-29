using Application.Common.DTOs;
using Application.Users.Interfaces;
using FluentValidation;
using MediatR;

namespace Application.Users.Queries;

public record GetUserByIdQuery(int Id) : IRequest<UserDto?>;

public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdQueryValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("User ID must be greater than 0.");
    }
}

public class GetUserByIdHandler(IUserReadRepository userRepository) : IRequestHandler<GetUserByIdQuery, UserDto?>
{
  public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        return await userRepository.GetByIdAsync((uint)request.Id);
    }
}