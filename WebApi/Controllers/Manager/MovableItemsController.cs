using Application.MovableItems.Commands;
using Application.MovableItems.Dtos;
using Application.MovableItems.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Manager;

[Route("api/manager/[controller]")]
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
    public async Task<IActionResult> GetMovableItem(uint id)
    {
        var movableItem = await mediator.Send(new GetMovableItemByIdQuery(id));
        return Ok(movableItem);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMovableItem([FromBody] CreateMovableItemDto createMovableItemDto)
    {
        var id = await mediator.Send(new CreateMovableItemCommand(createMovableItemDto));
        return CreatedAtAction(nameof(GetMovableItem), new { id }, null);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMovableItem(uint id, [FromBody] UpdateMovableItemDto updateMovableItemDto)
    {
        var command = new UpdateMovableItemCommand(id, updateMovableItemDto);
        await mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovableItem(uint id)
    {
        var command = new DeleteMovableItemCommand(id);
        await mediator.Send(command);
        return NoContent();
    }
}
