using FluentValidation;
using MediatR;
using ItTrAp.InventoryService.Application.Interfaces.Services;
using ItTrAp.InventoryService.Application.DTOs.Categories;

namespace ItTrAp.InventoryService.Application.Commands.Categories;

public record CreateCategoryCommand(CreateCategoryDto Category) : IRequest<uint>;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Category.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(100)
            .WithMessage("Name must be at most 100 characters long.");
    }
}

public class CreateCategoryHandler(ICategoryService categoryService) : IRequestHandler<CreateCategoryCommand, uint>
{
    public async Task<uint> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        return await categoryService.CreateAsync(request.Category, cancellationToken);
    }
}