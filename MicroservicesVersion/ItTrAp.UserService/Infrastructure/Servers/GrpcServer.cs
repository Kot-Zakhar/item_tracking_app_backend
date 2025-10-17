using Grpc.Core;
using ItTrAp.UserService.Infrastructure.Protos;
using MediatR;

namespace ItTrAp.UserService.Infrastructure.Servers;

public class GrpcServer : UserServer.UserServerBase
{
    private readonly ILogger<GrpcServer> _logger;
    private readonly IMediator _mediator;

    public GrpcServer(ILogger<GrpcServer> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public override async Task<GetUsersByIdsResponse> GetUsersByIds(GetUsersByIdsRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Received GetUsersByIds request with {Count} IDs", request.Ids.Count);

        var query = new Application.Queries.GetUsersByIdsQuery(request.Ids.ToList());
        var users = await _mediator.Send(query);

        var response = new GetUsersByIdsResponse();
        response.Users.AddRange(users.Select(user => new User
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Phone = user.Phone,
            Avatar = user.Avatar
        }));

        _logger.LogInformation("Returning {Count} users in response", response.Users.Count);
        return response;
    }
}