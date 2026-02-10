using ItTrAp.UserService.Application.Commands;
using ItTrAp.UserService.Application.DTOs;
using ItTrAp.UserService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItTrAp.UserService.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class UsersController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] string? search, [FromQuery] int? top)
    {
        var query = new GetAllFilteredUsersQuery(search, top);
        var users = await mediator.Send(query);

        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(uint id)
    {
        var user = await mediator.Send(new GetUserByIdQuery(id));
        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUserDto)
    {
        var id = await mediator.Send(new CreateUserCommand(createUserDto));
        return CreatedAtAction(nameof(GetUser), new { id }, null);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(uint id, [FromBody] UpdateUserDto updateUserDto)
    {
        var command = new UpdateUserCommand(id, updateUserDto);
        await mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(uint id)
    {
        var command = new DeleteUserCommand(id);
        await mediator.Send(command);
        return NoContent();
    }
}
