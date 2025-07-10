using Application.MovableItems.Commands;
using Application.MovableItems.DTOs;
using Application.MovableItems.Queries;
using Infrastructure.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Auth;

namespace WebApi.Controllers.Manager;

[Route("api/manager/v1/items")]
[Authorize]
[ApiController]
public class MovableItemsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [HasPermission(SecurityConstants.Permissions.MovableItems.List)]
    public async Task<IActionResult> GetMovableItems([FromQuery] MovableItemFiltersDto filters)
    {
        var movableItems = await mediator.Send(new GetAllFilteredMovableItemsQuery(filters));
        return Ok(movableItems);
    }

    [HttpGet("{id}")]
    [HasPermission(SecurityConstants.Permissions.MovableItems.Get)]
    public async Task<IActionResult> GetMovableItem(uint id)
    {
        var movableItem = await mediator.Send(new GetMovableItemByIdQuery(id));
        return Ok(movableItem);
    }

    [HttpPost]
    [HasPermission(SecurityConstants.Permissions.MovableItems.Create)]
    public async Task<IActionResult> CreateMovableItem([FromBody] CreateMovableItemDto createMovableItemDto)
    {
        var id = await mediator.Send(new CreateMovableItemCommand(createMovableItemDto));
        return CreatedAtAction(nameof(GetMovableItem), new { id }, null);
    }

    [HttpPut("{id}")]
    [HasPermission(SecurityConstants.Permissions.MovableItems.Update)]
    public async Task<IActionResult> UpdateMovableItem(uint id, [FromBody] UpdateMovableItemDto updateMovableItemDto)
    {
        var command = new UpdateMovableItemCommand(id, updateMovableItemDto);
        await mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [HasPermission(SecurityConstants.Permissions.MovableItems.Delete)]
    public async Task<IActionResult> DeleteMovableItem(uint id)
    {
        var command = new DeleteMovableItemCommand(id);
        await mediator.Send(command);
        return NoContent();
    }
}
