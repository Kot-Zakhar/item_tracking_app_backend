using Application.UserSelfManagement.Commands;
using Application.UserSelfManagement.DTOs;
using Application.UserSelfManagement.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Mobile;

[ApiController]
[Authorize]
[Route("api/mobile/v1/me")]
public class MeController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetSelfInfo(CancellationToken cancellationToken)
    {
        var userId = User.GetId();

        var query = new GetUserSelfQuery(userId);

        var user = await mediator.Send(query, cancellationToken);

        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateSelfInfo([FromBody] UpdateUserSelfDto request, CancellationToken cancellationToken)
    {
        var command = new UpdateUserSelfCommand(User.GetId(), request);

        await mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpPut("password")]
    public async Task<IActionResult> UpdateSelfPassword([FromBody] UpdateUserSelfPasswordDto request, CancellationToken cancellationToken)
    {
        var command = new UpdateUserSelfPasswordCommand(User.GetId(), request);

        await mediator.Send(command, cancellationToken);

        return NoContent();
    }
}