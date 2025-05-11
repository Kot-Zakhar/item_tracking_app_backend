using Application.Common.ViewModels;
using Application.Users.Interfaces;
using MediatR;

namespace Application.Users.Queries;

public record GetAllFilteredUsersQuery(string? Search, int? Top) : IRequest<List<UserViewModel>>;

public class GetAllFilteredUsersHandler(IUserReadRepository usersRepository) : IRequestHandler<GetAllFilteredUsersQuery, List<UserViewModel>>
{
    public async Task<List<UserViewModel>> Handle(GetAllFilteredUsersQuery request, CancellationToken cancellationToken)
    {
        return await usersRepository.GetAllFiltered(request.Search, request.Top);
    }
}
