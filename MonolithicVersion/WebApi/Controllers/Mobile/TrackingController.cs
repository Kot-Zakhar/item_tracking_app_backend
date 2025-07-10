using Application.Reservations.Commands;
using Application.Reservations.Queries;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Mobile;

[ApiController]
[Authorize]
[Route("api/mobile/v1/tracking")]
public class TrackingController(IMediator mediator) : ControllerBase
{
    [HttpGet("/item-instances")]
    public async Task<IActionResult> GetAssociatedItemInstances(CancellationToken cancellationToken)
    {
        uint userId = 1; // TODO: UserId is taken from jwt
        var instances = await mediator.Send(new GetAssociatedItemInstancesQuery(userId), cancellationToken);
        return Ok(instances);
    }


    public struct ChangeInstanceStatusByIdParam { public MovableInstanceStatus Status { get; set; } }

    // TODO: Rename this endpoint to CancelBooking in mobile API
    [HttpPut("/instances/{instanceId}/status")]
    public async Task<IActionResult> ChangeInstanceStatusById(uint instanceId, [FromBody] ChangeInstanceStatusByIdParam param, CancellationToken cancellationToken)
    {
        var issuerId = User.GetId();

        if (param.Status != MovableInstanceStatus.Available)
            return BadRequest("Only 'Available' status can be set via this endpoint.");

        var command = new CancelBookingCommand(issuerId, instanceId);

        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    public struct ChangeInstanceStatusByCodeParam
    {
        public MovableInstanceStatus Status { get; set; }
        public Guid LocationCode { get; set; }
    }

    [HttpPut("/instances/c{instanceCode}/status")]
    public async Task<IActionResult> ChangeInstanceStatusByCode(Guid instanceCode, [FromBody] ChangeInstanceStatusByCodeParam param, CancellationToken cancellationToken)
    {
        if (param.Status == MovableInstanceStatus.Booked)
        {
            // TODO: booking of concrete instance is to be implemented
            return BadRequest("Booking is not supported in mobile API.");
        }

        var issuerId = User.GetId();

        if (param.Status == MovableInstanceStatus.Available)
        {
            if (param.LocationCode == Guid.Empty)
            {
                return BadRequest("Location code is required for releasing the item.");
            }

            var command = new ReleaseByCodeCommand(issuerId, instanceCode, param.LocationCode);
            await mediator.Send(command, cancellationToken);
            return NoContent();
        }

        if (param.Status == MovableInstanceStatus.Taken)
        {
            var command = new TakeByCodeCommand(issuerId, instanceCode);
            await mediator.Send(command, cancellationToken);
            return NoContent();
        }

        return BadRequest("Invalid status.");
    }

    public struct BookAnyInstanceInLocationParam { public uint LocationId { get; set; } }

    // TODO: Rename this endpoint in v2
    [HttpPut("/item-instances/{itemId}/book-in-room")]
    public async Task<IActionResult> BookAnyInstanceInLocation(uint itemId, [FromBody] BookAnyInstanceInLocationParam param, CancellationToken cancellationToken)
    {
        var issuerId = User.GetId();
        var command = new BookAnyInstanceInLocationCommand(issuerId, issuerId, itemId, param.LocationId);
        var instanceId = await mediator.Send(command, cancellationToken);
        return Ok(instanceId);
    }
}