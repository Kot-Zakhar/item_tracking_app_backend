using Application.Categories.Commands;
using Application.Categories.Dtos;
using Application.Categories.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Manager;

[Route("api/manager/[controller]")]
[ApiController]
public class CategoriesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCategoryTree()
    {
        var categories = await mediator.Send(new GetDetailedCategoryTreeQuery());
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryTreeFromNode(uint id)
    {
        var categories = await mediator.Send(new GetDetailedCategoryTreeFromNodeQuery(id));
        return Ok(categories);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto body)
    {
        var command = new CreateCategoryCommand(body);
        var categoryId = await mediator.Send(command);
        return CreatedAtAction(nameof(GetCategoryTree), new { id = categoryId }, null);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(uint id, [FromBody] UpdateCategoryDto body)
    {
        var command = new UpdateCategoryCommand(id, body);
        await mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(uint id)
    {
        var command = new DeleteCategoryCommand(id);
        await mediator.Send(command);
        return NoContent();
    }
}