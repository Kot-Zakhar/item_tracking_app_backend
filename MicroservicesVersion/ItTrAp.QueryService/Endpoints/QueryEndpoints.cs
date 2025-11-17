using ItTrAp.QueryService.Application.Common;
using ItTrAp.QueryService.Application.Filters;
using ItTrAp.QueryService.Application.Interfaces;
using ItTrAp.QueryService.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace ItTrAp.QueryService.Endpoints;

public static class QueryEndpoints
{
    private static readonly int _defaultPageSize = 10;
    public static void MapQueryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/query")
            .RequireAuthorization();

        group.MapGet("/", () => Results.Ok(new { message = "QueryEndpoints is working." }))
            .WithTags("Query")
            .WithName("GetQueryStatus")
            .WithSummary("Checks the status of the Query service.")
            .WithDescription("This endpoint returns a simple message indicating that the Query service is operational.")
            .Produces(StatusCodes.Status200OK);

        group.MapGet("/locations", GetLocations)
            .WithTags("Locations")
            .WithName("GetLocations")
            .WithSummary("Retrieves a list of locations with details.")
            .WithDescription("This endpoint retrieves a paginated and filtered list of locations along with their details.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

        group.MapGet("/items", GetMovableItems)
            .WithTags("Movable Items")
            .WithName("GetMovableItems")
            .WithSummary("Retrieves a list of movable items with details.")
            .WithDescription("This endpoint retrieves a paginated and filtered list of movable items along with their details.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

        group.MapGet("/items/{itemId}/instances", GetMovableInstances)
            .WithTags("Movable Instances")
            .WithName("GetMovableInstances")
            .WithSummary("Retrieves a list of movable instances for a specific item.")
            .WithDescription("This endpoint retrieves a paginated and filtered list of movable instances for the specified item ID.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

        group.MapGet("/users", GetUsers)
            .WithTags("Users")
            .WithName("GetUsers")
            .WithSummary("Retrieves a list of users with details.")
            .WithDescription("This endpoint retrieves a paginated and filtered list of users along with their details.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);
    }

    public static async Task<IResult> GetLocations(
        [FromQuery] int? pageIndex,
        [FromQuery] int? pageSize,
        [FromQuery] string? search,
        [FromQuery] sbyte? floor,
        [FromQuery] bool? withItemsOnly,
        [FromServices] IQueryService queryService)
    {
        var query = new PaginatedFilteredQuery<LocationFiltersDto>
        {
            PageIndex = pageIndex ?? 0,
            PageSize = pageSize ?? _defaultPageSize,
            Filters = new LocationFiltersDto
            {
                Search = search,
                Floor = floor,
                WithItemsOnly = withItemsOnly
            }
        };
        var result = await queryService.GetLocationsWithDetailsAsync(query);
        return Results.Ok(result);
    }

    public static async Task<IResult> GetMovableItems(
        [FromQuery] int? pageIndex,
        [FromQuery] int? pageSize,
        [FromQuery] MovableInstanceStatus? status,
        [FromQuery] uint[]? categoryIds,
        [FromQuery] uint[]? locationIds,
        [FromQuery] string? search,
        [FromQuery] uint[]? userIds,
        [FromServices] IQueryService queryService)
    {
        var query = new PaginatedFilteredQuery<MovableItemFiltersDto>
        {
            PageIndex = pageIndex ?? 0,
            PageSize = pageSize ?? _defaultPageSize,
            Filters = new MovableItemFiltersDto
            {
                Status = status,
                Search = search,
                CategoryIds = categoryIds?.ToList(),
                LocationIds = locationIds?.ToList(),
                UserIds = userIds?.ToList(),
            }
        };
        var result = await queryService.GetMovableItemsWithDetailsAsync(query);
        return Results.Ok(result);
    }

    public static async Task<IResult> GetMovableInstances(
        [FromRoute] Guid itemId,
        [FromQuery] int? pageIndex,
        [FromQuery] int? pageSize,
        [FromQuery] MovableInstanceStatus? status,
        [FromQuery] uint? locationId,
        [FromQuery] uint[]? userIds,
        [FromServices] IQueryService queryService)
    {
        var query = new PaginatedFilteredQuery<MovableInstanceFiltersDto>
        {
            PageIndex = pageIndex ?? 0,
            PageSize = pageSize ?? _defaultPageSize,
            Filters = new MovableInstanceFiltersDto
            {
                Status = status,
                LocationId = locationId,
                UserIds = userIds?.ToList() ?? new List<uint>(),
            }
        };
        var result = await queryService.GetMovableInstancesAsync(itemId, query);
        return Results.Ok(result);
    }

    public static async Task<IResult> GetUsers(
        [FromQuery] int? pageIndex,
        [FromQuery] int? pageSize,
        [FromQuery] string? search,
        [FromQuery] bool? hasInstances,
        [FromServices] IQueryService queryService)
    {
        var query = new PaginatedFilteredQuery<UserFiltersDto>
        {
            PageIndex = pageIndex ?? 0,
            PageSize = pageSize ?? _defaultPageSize,
            Filters = new UserFiltersDto
            {
                Search = search,
                HasInstances = hasInstances
            }
        };
        var result = await queryService.GetUsersWithDetailsAsync(query);
        return Results.Ok(result);
    }
}