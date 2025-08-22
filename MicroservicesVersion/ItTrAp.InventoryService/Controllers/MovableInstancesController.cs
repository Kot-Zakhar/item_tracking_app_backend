using ItTrAp.InventoryService.Application.Commands.MovableInstances;
using ItTrAp.InventoryService.Application.Queries.MovableInstances;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItTrAp.InventoryService.Controllers;


[Route("api/v1/items/{itemId}/instances")]
[Authorize]
[ApiController]
public class MovableInstancesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetMovableInstances(Guid itemId)
    {
        var movableInstances = await mediator.Send(new GetAllMovableInstancesQuery(itemId));
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
        var id = await mediator.Send(new CreateMovableInstanceCommand(itemId));
        return CreatedAtAction(nameof(GetMovableInstance), new { itemId, id }, null);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovableInstance(Guid itemId, uint id)
    {
        var command = new DeleteMovableInstanceCommand(itemId, id);
        await mediator.Send(command);
        return NoContent();
    }
}