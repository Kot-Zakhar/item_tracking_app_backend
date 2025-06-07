using Application.MovableInstances.Commands;
using Application.MovableInstances.DTOs;
using Application.MovableInstances.Queries;
using Application.Reservations.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Manager;

[Route("api/manager/items/{itemId}/instances")]
[Authorize]
[ApiController]
public class MovableInstancesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetMovableInstances(uint itemId, [FromQuery] MovableInstanceFiltersDto filters)
    {
        var movableInstances = await mediator.Send(new GetAllFilteredMovableInstancesQuery(itemId, filters));
        return Ok(movableInstances);
    }

    [HttpGet("{id}")]
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
    public async Task<IActionResult> CreateMovableInstance(uint itemId)
    {
        var id = await mediator.Send(new CreateMovableInstanceCommand(itemId));
        return CreatedAtAction(null, new { itemId, id }, null); // TODO: Specify the endpoint that gets this particular instance
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovableInstance(uint itemId, uint id)
    {
        var command = new DeleteMovableInstanceCommand(itemId, id);
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id}/book")]
    public async Task<IActionResult> BookMovableInstance(uint itemId, uint id, [FromBody] BookCommand command)
    {
        command = command with { InstanceId = id };
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id}/cancel")]
    public async Task<IActionResult> CancelBookingOfMovableInstance(uint itemId, uint id, [FromBody] CancelBookingCommand command)
    {
        command = command with { InstanceId = id };
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id}/take")]
    public async Task<IActionResult> TakeMovableInstance(uint itemId, uint id, [FromBody] TakeCommand command)
    {
        command = command with { InstanceId = id };
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id}/release")]
    public async Task<IActionResult> ReleaseMovableInstance(uint itemId, uint id, [FromBody] ReleaseCommand command)
    {
        command = command with { InstanceId = id };
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id}/move")]
    public async Task<IActionResult> MoveMovableInstance(uint itemId, uint id, [FromBody] MoveCommand command)
    {
        command = command with { InstanceId = id };
        await mediator.Send(command);
        return NoContent();
    }
}