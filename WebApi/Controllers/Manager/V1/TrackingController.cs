namespace WebApi.Controllers.Manager;

using Application.Reservations.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// version 1 of manager tracking API

[Route("api/manager/v1/tracking")]
[Authorize]
[ApiController]
public class TrackingController(IMediator mediator) : ControllerBase
{
    [HttpPut("instances/{instanceId}")]
    public async Task<IActionResult> ChangeInstanceStatus(uint instanceId, [FromBody] ChangeInstanceStatusCommand body, CancellationToken cancellationToken)
    {
        var command = body with { InstanceId = instanceId };

        await mediator.Send(command, cancellationToken);
        return NoContent();
    }
}