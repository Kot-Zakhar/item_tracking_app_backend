using Application.Categories.DTOs;
using Application.Categories.Interfaces;
using FluentValidation;
using MediatR;

namespace Application.Categories.Commands;

public record UpdateCategoryCommand(uint Id, UpdateCategoryDto Category) : IRequest;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => new { x.Category.Name, x.Category.Icon })
            .Must(fields => !(string.IsNullOrEmpty(fields.Name) && string.IsNullOrEmpty(fields.Icon)))
            .WithMessage("At least one of Name or Icon must be provided.");

        RuleFor(x => (int)x.Id).NotEqual(0).WithMessage("Category ID must be greater than 0.");

        RuleFor(x => x.Category.Name)
            .MaximumLength(100)
            .WithMessage("Name must be at most 100 characters long.");
    }
}

public class UpdateCategoryHandler(ICategoryService categoryService) : IRequestHandler<UpdateCategoryCommand>
{
    public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        => await categoryService.UpdateAsync(request.Id, request.Category, cancellationToken);
}