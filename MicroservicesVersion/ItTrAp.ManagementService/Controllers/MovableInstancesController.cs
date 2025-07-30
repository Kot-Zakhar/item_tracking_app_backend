using ItTrAp.ManagementService.Application.Commands.MovableInstances;
using ItTrAp.ManagementService.Application.DTOs.MovableInstances;
using ItTrAp.ManagementService.Application.Commands.Reservations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ItTrAp.ManagementService.Application.Queries.MovableInstances;

namespace ItTrAp.ManagementService.Controllers;

[Route("api/v1/items/{itemId}/instances")]
[Authorize]
[ApiController]
public class MovableInstancesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetMovableInstances(Guid itemId, [FromQuery] MovableInstanceFiltersDto filters)
    {
        var movableInstances = await mediator.Send(new GetAllFilteredMovableInstancesQuery(itemId, filters));
        return Ok(movableInstances);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMovableInstance(Guid itemId, uint id)
    {
        var movableInstance = await mediator.Send(new GetMovableInstanceByIdQuery(itemId, id));
        if (movableInstance == null)
        {
            return NotFound();
        }
        return Ok(movableInstance);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMovableInstance(Guid itemId)
    {
        var issuerId = User.GetId();
        var id = await mediator.Send(new CreateMovableInstanceCommand(itemId, issuerId));
        return CreatedAtAction(nameof(GetMovableInstance), new { itemId, id }, null);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovableInstance(Guid itemId, uint id)
    {
        var issuerId = User.GetId();
        var command = new DeleteMovableInstanceCommand(itemId, id, issuerId);
        await mediator.Send(command);
        return NoContent();
    }

    public record BookRequest(uint UserId);

    [HttpPut("{id}/book")]
    public async Task<IActionResult> BookMovableInstance(Guid itemId, uint id, [FromBody] BookRequest request)
    {
        var issuerId = User.GetId();
        var command = new BookCommand(issuerId, request.UserId, id);
        command = command with { InstanceId = id };
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id}/cancel")]
    public async Task<IActionResult> CancelBookingOfMovableInstance(Guid itemId, uint id)
    {
        var userId = User.GetId();
        var command = new CancelBookingCommand(userId, id);
        await mediator.Send(command);
        return NoContent();
    }

    public record AssignRequest(uint UserId);

    [HttpPut("{id}/assign")]
    public async Task<IActionResult> AssignMovableInstance(Guid itemId, uint id, [FromBody] AssignRequest request)
    {
        var issuerId = User.GetId();
        var command = new AssignCommand(issuerId, request.UserId, id);
        await mediator.Send(command);
        return NoContent();
    }

    public record ReleaseRequest(uint LocationId);

    [HttpPut("{id}/release")]
    public async Task<IActionResult> ReleaseMovableInstance(Guid itemId, uint id, [FromBody] ReleaseRequest request)
    {
        var userId = User.GetId();
        var command = new ReleaseCommand(userId, id, request.LocationId);
        await mediator.Send(command);
        return NoContent();
    }

    public record MoveRequest(uint LocationId);

    [HttpPut("{id}/move")]
    public async Task<IActionResult> MoveMovableInstance(Guid itemId, uint id, [FromBody] MoveRequest request)
    {
        var userId = User.GetId();
        var command = new MoveCommand(userId, id, request.LocationId);
        await mediator.Send(command);
        return NoContent();
    }

    // [HttpGet("{id}/history")] // TODO: implement history endpoint
}
