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
    [HasPermission(PredefinedPermissions.GetAllFilteredMovableInstances)]
    public async Task<IActionResult> GetMovableInstances(uint itemId, [FromQuery] MovableInstanceFiltersDto filters)
    {
        var movableInstances = await mediator.Send(new GetAllFilteredMovableInstancesQuery(itemId, filters));
        return Ok(movableInstances);
    }

    [HttpGet("{id}")]
    [HasPermission(PredefinedPermissions.GetMovableInstanceById)]
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
    [HasPermission(PredefinedPermissions.CreateMovableInstance)]
    public async Task<IActionResult> CreateMovableInstance(uint itemId)
    {
        var id = await mediator.Send(new CreateMovableInstanceCommand(itemId));
        return CreatedAtAction(null, new { itemId, id }, null); // TODO: Specify the endpoint that gets this particular instance
    }

    [HttpDelete("{id}")]
    [HasPermission(PredefinedPermissions.DeleteMovableInstance)]
    public async Task<IActionResult> DeleteMovableInstance(uint itemId, uint id)
    {
        var command = new DeleteMovableInstanceCommand(itemId, id);
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id}/book")]
    [HasPermission(PredefinedPermissions.BookMovableInstance)]
    public async Task<IActionResult> BookMovableInstance(uint itemId, uint id, [FromBody] BookCommand command)
    {
        command = command with { InstanceId = id };
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id}/cancel")]
    [HasPermission(PredefinedPermissions.CancelBookingOfMovableInstance)]
    public async Task<IActionResult> CancelBookingOfMovableInstance(uint itemId, uint id, [FromBody] CancelBookingCommand command)
    {
        command = command with { InstanceId = id };
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id}/take")]
    [HasPermission(PredefinedPermissions.TakeMovableInstance)]
    public async Task<IActionResult> TakeMovableInstance(uint itemId, uint id, [FromBody] TakeCommand command)
    {
        command = command with { InstanceId = id };
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id}/release")]
    [HasPermission(PredefinedPermissions.ReleaseMovableInstance)]
    public async Task<IActionResult> ReleaseMovableInstance(uint itemId, uint id, [FromBody] ReleaseCommand command)
    {
        command = command with { InstanceId = id };
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id}/move")]
    [HasPermission(PredefinedPermissions.MoveMovableInstance)]
    public async Task<IActionResult> MoveMovableInstance(uint itemId, uint id, [FromBody] MoveCommand command)
    {
        var userId = User.GetId();

        // Move action is performed by manager
        command = command with { InstanceId = id, UserId = userId };

        await mediator.Send(command);
        return NoContent();
    }

    // [HttpGet("{id}/history")] // TODO: implement history endpoint

    [HttpGet("{id}/qr")]
    [HasPermission(PredefinedPermissions.GetMovableInstanceQrCode)]
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
