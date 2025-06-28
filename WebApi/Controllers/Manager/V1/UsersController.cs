using Application.Users.Commands;
using Application.Users.DTOs;
using Application.Users.Queries;
using Infrastructure.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Auth;

namespace WebApi.Controllers.Manager;

[Route("api/manager/v1/[controller]")]
[Authorize]
[ApiController]
public class UsersController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id}")]
    [HasPermission(SecurityConstants.Permissions.Users.Get)]
    public async Task<IActionResult> GetUser(uint id)
    {
        var user = await mediator.Send(new GetUserByIdQuery(id));
        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpGet]
    [HasPermission(SecurityConstants.Permissions.Users.List)]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await mediator.Send(new GetAllFilteredUsersQuery(null, null));
        return Ok(users);
    }

    [HttpPost]
    [HasPermission(SecurityConstants.Permissions.Users.Create)]
    public async Task<IActionResult> CreateUser(CreateUserDto user)
    {
        var command = new CreateUserCommand(user);
        var id = await mediator.Send(command);
        if (id == 0) return BadRequest("Failed to create user.");
        return CreatedAtAction(nameof(GetUser), new { id }, new { id });
    }

    [HttpPut("{id}")]
    [HasPermission(SecurityConstants.Permissions.Users.Update)]
    public async Task<IActionResult> UpdateUser(uint id, UpdateUserDto user)
    {
        var command = new UpdateUserCommand(id, user);
        await mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [HasPermission(SecurityConstants.Permissions.Users.Delete)]
    public async Task<IActionResult> DeleteUser(uint id)
    {
        await mediator.Send(new DeleteUserCommand(id));
        return NoContent();
    }

    [HttpPut("{id}/password")]
    [HasPermission(SecurityConstants.Permissions.Users.UpdatePassword)]
    public async Task<IActionResult> ResetPassword(uint id, ResetUserPasswordDto passwords)
    {
        var command = new ResetUserPasswordCommand(id, passwords);
        await mediator.Send(command);
        return NoContent();
    }
}

