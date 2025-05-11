using Application.Common.ViewModels;
using Application.Users.Interfaces;
using FluentValidation;
using MediatR;

namespace Application.Users.Queries;

public record GetUserByIdQuery(int Id) : IRequest<UserViewModel?>;

public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdQueryValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("User ID must be greater than 0.");
    }
}

public class GetUserByIdHandler(IUserReadRepository userRepository) : IRequestHandler<GetUserByIdQuery, UserViewModel?>
{
  public async Task<UserViewModel?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        return await userRepository.GetByIdAsync((uint)request.Id);
    }
}