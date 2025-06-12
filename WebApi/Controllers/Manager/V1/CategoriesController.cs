using Application.Categories.Commands;
using Application.Categories.DTOs;
using Application.Categories.Queries;
using Infrastructure.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Auth;

namespace WebApi.Controllers.Manager;

[Route("api/manager/v1/[controller]")]
[Authorize]
[ApiController]
public class CategoriesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [HasPermission(PredefinedPermissions.GetCategoryTree)]
    public async Task<IActionResult> GetCategoryTree()
    {
        var categories = await mediator.Send(new GetDetailedCategoryTreeQuery());
        return Ok(categories);
    }

    [HttpGet("{id}")]
    [HasPermission(PredefinedPermissions.GetCategoryTreeFromNode)]
    public async Task<IActionResult> GetCategoryTreeFromNode(uint id)
    {
        var categories = await mediator.Send(new GetDetailedCategoryTreeFromNodeQuery(id));
        return Ok(categories);
    }

    [HttpPost]
    [HasPermission(PredefinedPermissions.CreateCategory)]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto body)
    {
        var command = new CreateCategoryCommand(body);
        var categoryId = await mediator.Send(command);
        return CreatedAtAction(nameof(GetCategoryTree), new { id = categoryId }, null);
    }

    [HttpPut("{id}")]
    [HasPermission(PredefinedPermissions.UpdateCategory)]
    public async Task<IActionResult> UpdateCategory(uint id, [FromBody] UpdateCategoryDto body)
    {
        var command = new UpdateCategoryCommand(id, body);
        await mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [HasPermission(PredefinedPermissions.DeleteCategory)]
    public async Task<IActionResult> DeleteCategory(uint id)
    {
        var command = new DeleteCategoryCommand(id);
        await mediator.Send(command);
        return NoContent();
    }
}