using Application.Common.DTOs;
using Application.Users.Interfaces;
using MediatR;

namespace Application.Users.Queries;

public record GetAllFilteredUsersQuery(string? Search, int? Top) : IRequest<List<UserDto>>;

public class GetAllFilteredUsersHandler(IUserReadRepository usersRepository) : IRequestHandler<GetAllFilteredUsersQuery, List<UserDto>>
{
    public async Task<List<UserDto>> Handle(GetAllFilteredUsersQuery request, CancellationToken cancellationToken)
    {
        return await usersRepository.GetAllFiltered(request.Search, request.Top);
    }
}
