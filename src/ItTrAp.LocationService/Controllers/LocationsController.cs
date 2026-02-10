using ItTrAp.LocationService.Application.Commands;
using ItTrAp.LocationService.Application.DTOs;
using ItTrAp.LocationService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItTrAp.LocationService.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public class LocationsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetLocations([FromQuery] LocationFiltersDto filters)
    {
        var locations = await mediator.Send(new GetAllFilteredLocationsQuery(filters));
        return Ok(locations);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetLocation(uint id)
    {
        var location = await mediator.Send(new GetLocationByIdQuery(id));
        if (location == null)
        {
            return NotFound();
        }
        return Ok(location);
    }

    [HttpPost]
    public async Task<IActionResult> CreateLocation([FromBody] CreateLocationDto createLocationDto)
    {
        var id = await mediator.Send(new CreateLocationCommand(createLocationDto));
        return CreatedAtAction(nameof(GetLocation), new { id }, null);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLocation(uint id, [FromBody] UpdateLocationDto updateLocationDto)
    {
        var command = new UpdateLocationCommand(id, updateLocationDto);
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