using Application.MovableInstances.Commands;
using Application.MovableInstances.DTOs;
using Application.MovableInstances.Queries;
using Application.Reservations.Commands;
using Infrastructure.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Auth;

namespace WebApi.Controllers.Manager;

[Route("api/manager/v1/items/{itemId}/instances")]
[Authorize]
[ApiController]
public class MovableInstancesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [HasPermission(SecurityConstants.Permissions.MovableInstances.List)]
    public async Task<IActionResult> GetMovableInstances(uint itemId, [FromQuery] MovableInstanceFiltersDto filters)
    {
        var movableInstances = await mediator.Send(new GetAllFilteredMovableInstancesQuery(itemId, filters));
        return Ok(movableInstances);
    }

    [HttpGet("{id}")]
    [HasPermission(SecurityConstants.Permissions.MovableInstances.Get)]
    public async Task<IActionResult> GetMovableInstance(uint itemId, uint id)
    {
        var movableInstance = await mediator.Send(new GetMovableInstanceByIdQuery(itemId, id));
        if (movableInstance == null)
        {
            return NotFound();
        }
        return Ok(movableInstance);
    }

    [HttpPost]
    [HasPermission(SecurityConstants.Permissions.MovableInstances.Create)]
    public async Task<IActionResult> CreateMovableInstance(uint itemId)
    {
        var id = await mediator.Send(new CreateMovableInstanceCommand(itemId));
        return CreatedAtAction(null, new { itemId, id }, null); // TODO: Specify the endpoint that gets this particular instance
    }

    [HttpDelete("{id}")]
    [HasPermission(SecurityConstants.Permissions.MovableInstances.Delete)]
    public async Task<IActionResult> DeleteMovableInstance(uint itemId, uint id)
    {
        var command = new DeleteMovableInstanceCommand(itemId, id);
        await mediator.Send(command);
        return NoContent();
    }

    public record BookRequest(uint UserId);

    [HttpPut("{id}/book")]
    [HasPermission(SecurityConstants.Permissions.MovableInstances.Book)]
    public async Task<IActionResult> BookMovableInstance(uint itemId, uint id, [FromBody] BookRequest request)
    {
        var issuerId = User.GetId();
        var command = new BookCommand(issuerId, request.UserId, id);
        command = command with { InstanceId = id };
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id}/cancel")]
    [HasPermission(SecurityConstants.Permissions.MovableInstances.CancelBooking)]
    public async Task<IActionResult> CancelBookingOfMovableInstance(uint itemId, uint id)
    {
        var userId = User.GetId();
        var command = new CancelBookingCommand(userId, id);
        await mediator.Send(command);
        return NoContent();
    }

    public record AssignRequest(uint UserId);

    [HttpPut("{id}/assign")]
    [HasPermission(SecurityConstants.Permissions.MovableInstances.Assign)]
    public async Task<IActionResult> AssignMovableInstance(uint itemId, uint id, [FromBody] AssignRequest request)
    {
        var issuerId = User.GetId();
        var command = new AssignCommand(issuerId, request.UserId, id);
        await mediator.Send(command);
        return NoContent();
    }

    public record ReleaseRequest(uint LocationId);

    [HttpPut("{id}/release")]
    [HasPermission(SecurityConstants.Permissions.MovableInstances.Release)]
    public async Task<IActionResult> ReleaseMovableInstance(uint itemId, uint id, [FromBody] ReleaseRequest request)
    {
        var userId = User.GetId();
        var command = new ReleaseCommand(userId, id, request.LocationId);
        await mediator.Send(command);
        return NoContent();
    }

    public record MoveRequest(uint LocationId);

    [HttpPut("{id}/move")]
    [HasPermission(SecurityConstants.Permissions.MovableInstances.Move)]
    public async Task<IActionResult> MoveMovableInstance(uint itemId, uint id, [FromBody] MoveRequest request)
    {
        var userId = User.GetId();
        var command = new MoveCommand(userId, id, request.LocationId);
        await mediator.Send(command);
        return NoContent();
    }

    // [HttpGet("{id}/history")] // TODO: implement history endpoint

    [HttpGet("{id}/qr")]
    [HasPermission(SecurityConstants.Permissions.MovableInstances.GetQrCode)]
    public async Task<IActionResult> GetMovableInstanceQrCode(uint itemId, uint id)
    {
        var qrCode = await mediator.Send(new GetMovableInstanceQrCodeQuery(itemId, id));
        if (qrCode == null)
        {
            return NotFound();
        }
        return File(qrCode, "image/png");
    }
}
