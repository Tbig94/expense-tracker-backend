using ExpenseTrackerApi.Application.Budgets.Queries.CheckBudgetOVerflow;
using FluentValidation;

namespace ExpenseTrackerApi.Application.Budgets.Queries.CheckBudgetOverflow;

public class CheckBudgetOverflowQueryValidator : AbstractValidator<CheckBudgetOverflowQuery>
{
    public CheckBudgetOverflowQueryValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("Category Id cannot be empty!");
    }
}
