using Application.Locations.Commands;
using Application.Locations.DTOs;
using Application.Locations.Queries;
using Infrastructure.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Auth;

namespace WebApi.Controllers.Manager;

[Route("api/manager/v1/[controller]")]
[Authorize]
[ApiController]
public class LocationsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [HasPermission(PredefinedPermissions.GetAllFilteredLocations)]
    public async Task<IActionResult> GetAllFiltered([FromQuery] LocationFiltersDto filter)
    {
        var locations = await mediator.Send(new GetAllFilteredLocationsQuery(filter));
        return Ok(locations);
    }

    [HttpGet("{id}")]
    [HasPermission(PredefinedPermissions.GetLocationById)]
    public async Task<IActionResult> GetById(uint id)
    {
        var locations = await mediator.Send(new GetLocationByIdQuery(id));
        return Ok(locations);
    }

    [HttpPost]
    [HasPermission(PredefinedPermissions.CreateLocation)]
    public async Task<IActionResult> CreateLocation([FromBody] CreateLocationDto body)
    {
        var command = new CreateLocationCommand(body);
        var LocationId = await mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = LocationId }, null);
    }

    [HttpPut("{id}")]
    [HasPermission(PredefinedPermissions.UpdateLocation)]
    public async Task<IActionResult> UpdateLocation(uint id, [FromBody] UpdateLocationDto body)
    {
        var command = new UpdateLocationCommand(id, body);
        await mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [HasPermission(PredefinedPermissions.DeleteLocation)]
    public async Task<IActionResult> DeleteLocation(uint id)
    {
        var command = new DeleteLocationCommand(id);
        await mediator.Send(command);
        return NoContent();
    }

    [HttpGet("{id}/qr")]
    [HasPermission(PredefinedPermissions.GetLocationQrCode)]
    public async Task<IActionResult> GetLocationQrCode(uint id)
    {
        var qrCode = await mediator.Send(new GetLocationQrCodeQuery(id));
        if (qrCode == null)
        {
            return NotFound();
        }
        return File(qrCode, "image/png");
    }
}