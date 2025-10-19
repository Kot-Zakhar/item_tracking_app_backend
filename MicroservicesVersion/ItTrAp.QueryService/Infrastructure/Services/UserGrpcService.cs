using ItTrAp.QueryService.Application.Responses;
using ItTrAp.QueryService.Infrastructure.Interfaces.Services;
using Microsoft.Extensions.Options;

namespace ItTrAp.QueryService.Infrastructure.Services;

public class UserGrpcService(ILogger<UserGrpcService> logger, IOptions<GlobalConfig> config) : IUserService
{
    private readonly string _userServiceAddress = config.Value.UserServiceAddress;

    public async Task<List<UserViewModel>> GetUsersByIdsAsync(List<uint> userIds, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Fetching users from UserService at {UserServiceAddress}", _userServiceAddress);

        try
        {
            using var userChannel = Grpc.Net.Client.GrpcChannel.ForAddress(_userServiceAddress);
            var userClient = new Protos.UserServer.UserServerClient(userChannel);

            var request = new Protos.GetUsersByIdsRequest();
            request.Ids.AddRange(userIds);

            var response = await userClient.GetUsersByIdsAsync(request, cancellationToken: cancellationToken);
            return response.Users.Select(user => new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                Avatar = user.Avatar
            }).ToList();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while fetching users");
            throw;
        }
    }

    public async Task<List<UserViewModel>> GetUsersAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Fetching all users from UserService at {UserServiceAddress}", _userServiceAddress);

        try
        {
            using var userChannel = Grpc.Net.Client.GrpcChannel.ForAddress(_userServiceAddress);
            var userClient = new Protos.UserServer.UserServerClient(userChannel);

            var request = new Protos.GetUsersRequest();

            var response = await userClient.GetUsersAsync(request, cancellationToken: cancellationToken);
            return response.Users.Select(user => new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                Avatar = user.Avatar
            }).ToList();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while fetching all users");
            throw;
        }
    }

}