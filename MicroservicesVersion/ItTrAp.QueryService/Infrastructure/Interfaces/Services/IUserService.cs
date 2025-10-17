using ItTrAp.QueryService.Application.Responses;

namespace ItTrAp.QueryService.Infrastructure.Interfaces.Services;

public interface IUserService
{
    Task<List<UserViewModel>> GetUsersByIdsAsync(List<uint> userIds, CancellationToken cancellationToken = default);
}