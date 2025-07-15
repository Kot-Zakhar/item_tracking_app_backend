using ItTrAp.InventoryService.Application.Interfaces.Services;
using MediatR;

namespace ItTrAp.InventoryService.Application.Commands.Categories;

public record DeleteCategoryCommand(uint Id) : IRequest;

public class DeleteCategoryHandler(ICategoryService categoryService) : IRequestHandler<DeleteCategoryCommand>
{
    public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        => await categoryService.DeleteAsync(request.Id, cancellationToken);
}