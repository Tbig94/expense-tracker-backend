using FluentValidation;

namespace ExpenseTrackerApi.Application.Categories.Commands.CreateCategory;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Category name cannot be empty!");

        RuleFor(x => x.Color)
            .NotEmpty()
            .WithMessage("Category color cannot be empty!");
    }
}
