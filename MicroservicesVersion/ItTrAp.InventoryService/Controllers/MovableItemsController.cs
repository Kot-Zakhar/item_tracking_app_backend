using ItTrAp.Commands.MovableItems;
using ItTrAp.InventoryService.Application.DTOs.MovableItems;
using ItTrAp.InventoryService.Application.Queries.MovableItems;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Manager;

[Route("api/v1/items")]
[Authorize]
[ApiController]
public class MovableItemsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetMovableItems([FromQuery] MovableItemFiltersDto filters)
    {
        var movableItems = await mediator.Send(new GetAllFilteredMovableItemsQuery(filters));
        return Ok(movableItems);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMovableItem(Guid id)
    {
        var movableItem = await mediator.Send(new GetMovableItemByIdQuery(id));
        if (movableItem == null)
        {
            return NotFound();
        }
        return Ok(movableItem);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMovableItem([FromBody] CreateMovableItemDto createMovableItemDto)
    {
        var id = await mediator.Send(new CreateMovableItemCommand(createMovableItemDto));
        return CreatedAtAction(nameof(GetMovableItem), new { id }, null);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMovableItem(Guid id, [FromBody] UpdateMovableItemDto updateMovableItemDto)
    {
        var command = new UpdateMovableItemCommand(id, updateMovableItemDto);
        await mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovableItem(Guid id)
    {
        var command = new DeleteMovableItemCommand(id);
        await mediator.Send(command);
        return NoContent();
    }
}
