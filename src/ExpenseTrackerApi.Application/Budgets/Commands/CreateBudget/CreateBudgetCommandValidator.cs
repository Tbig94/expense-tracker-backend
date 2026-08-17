using FluentValidation;

namespace ExpenseTrackerApi.Application.Budgets.Commands.CreateBudget;

public class CreateBudgetCommandValidator : AbstractValidator<CreateBudgetCommand>
{
    public CreateBudgetCommandValidator()
    {
        RuleFor(x => x.Budget)
            .NotNull()
            .WithMessage("Budget data is requried!");

        RuleFor(x => x.Budget.CategoryId)
            .NotEmpty()
            .WithMessage("Category ID cannot be empty");

        RuleFor(x => x.Budget.LimitAmount)
            .Must(x => x > 0)
            .WithMessage("Limit amount must be positive or null");

        RuleFor(x => x.Budget.Month)
            .InclusiveBetween(1, 12)
            .WithMessage("Month must be between 1 and 12");

        RuleFor(x => x.Budget.Year)
            .LessThanOrEqualTo(DateTime.UtcNow.Year)
            .WithMessage("Year cannot be more than current year");
    }
}
