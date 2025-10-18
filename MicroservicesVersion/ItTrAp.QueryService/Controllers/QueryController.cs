using ItTrAp.QueryService.Application.Common;
using ItTrAp.QueryService.Application.Filters;
using ItTrAp.QueryService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

// TODO: use MediatR to handle requests

namespace ItTrAp.QueryService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QueryController(IQueryService queryService) : ControllerBase
    {
        // GET: api/query
        [HttpGet]
        public IActionResult Get()
        {
            // Placeholder for query logic
            return Ok(new { message = "QueryController is working." });
        }

        [HttpGet("locations")]
        public async Task<IActionResult> GetLocations([FromQuery] PaginatedFilteredQuery<LocationFiltersDto> query)
        {
            var result = await queryService.GetLocationsWithDetailsAsync(query);
            return Ok(result);
        }

        [HttpGet("items")]
        public async Task<IActionResult> GetMovableItems([FromQuery] PaginatedFilteredQuery<MovableItemFiltersDto> query)
        {
            var result = await queryService.GetMovableItemsWithDetailsAsync(query);
            return Ok(result);
        }

        [HttpGet("items/{itemId}/instances")]
        public async Task<IActionResult> GetMovableInstances([FromRoute] Guid itemId, [FromQuery] PaginatedFilteredQuery<MovableInstanceFiltersDto> query)
        {
            var result = await queryService.GetMovableInstancesAsync(itemId, query);
            return Ok(result);
        }
    }
}