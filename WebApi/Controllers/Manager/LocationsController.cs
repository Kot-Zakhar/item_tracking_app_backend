using Application.Locations.Commands;
using Application.Locations.DTOs;
using Application.Locations.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Manager;

[Route("api/manager/[controller]")]
[ApiController]
public class LocationsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllFiltered([FromQuery] LocationFiltersDto filter)
    {
        var locations = await mediator.Send(new GetAllFilteredLocationsQuery(filter));
        return Ok(locations);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(uint id)
    {
        var locations = await mediator.Send(new GetLocationByIdQuery(id));
        return Ok(locations);
    }

    [HttpPost]
    public async Task<IActionResult> CreateLocation([FromBody] CreateLocationDto body)
    {
        var command = new CreateLocationCommand(body);
        var LocationId = await mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = LocationId }, null);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLocation(uint id, [FromBody] UpdateLocationDto body)
    {
        var command = new UpdateLocationCommand(id, body);
        await mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLocation(uint id)
    {
        var command = new DeleteLocationCommand(id);
        await mediator.Send(command);
        return NoContent();
    }
}