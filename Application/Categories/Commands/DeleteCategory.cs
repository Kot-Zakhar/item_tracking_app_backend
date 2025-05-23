using Application.Categories.Interfaces;
using MediatR;

namespace Application.Categories.Commands;

public record DeleteCategoryCommand(uint Id) : IRequest;

public class DeleteCategoryHandler(ICategoryService categoryService) : IRequestHandler<DeleteCategoryCommand>
{
    public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        => await categoryService.DeleteAsync(request.Id, cancellationToken);
}