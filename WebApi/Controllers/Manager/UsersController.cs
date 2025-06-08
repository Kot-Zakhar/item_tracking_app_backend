using Application.Users.Commands;
using Application.Users.Queries;
using Infrastructure.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Auth;

namespace WebApi.Controllers.Manager;

[Route("api/manager/[controller]")]
[Authorize]
[ApiController]
public class UsersController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id}")]
    [HasPermission(PredefinedPermissions.GetUser)]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await mediator.Send(new GetUserByIdQuery(id));
        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpGet]
    [HasPermission(PredefinedPermissions.GetAllUsers)]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await mediator.Send(new GetAllFilteredUsersQuery(null, null));
        return Ok(users);
    }

    [HttpPost]
    [HasPermission(PredefinedPermissions.CreateUser)]
    public async Task<IActionResult> CreateUser(CreateUserCommand command)
    {
        var id = await mediator.Send(command);
        if (id == 0) return BadRequest("Failed to create user.");
        return CreatedAtAction(nameof(GetUser), new { id }, new { id });
    }

    [HttpPut("{id}")]
    [HasPermission(PredefinedPermissions.UpdateUser)]
    public async Task<IActionResult> UpdateUser(int id, UpdateUserCommand command)
    {
        var createCommand = command with { Id = id };
        await mediator.Send(createCommand);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [HasPermission(PredefinedPermissions.DeleteUser)]
    public async Task<IActionResult> DeleteUser(int id)
    {
        await mediator.Send(new DeleteUserCommand(id));
        return NoContent();
    }

    [HttpPut("{id}/password")]
    [HasPermission(PredefinedPermissions.UpdateUserPassword)]
    public async Task<IActionResult> UpdatePassword(int id, UpdatePasswordCommand command)
    {
        var updatedCommand = command with { Id = id };
        await mediator.Send(updatedCommand);
        return NoContent();
    }
}

