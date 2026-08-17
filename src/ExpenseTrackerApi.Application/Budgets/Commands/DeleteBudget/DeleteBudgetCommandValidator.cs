using FluentValidation;

namespace ExpenseTrackerApi.Application.Budgets.Commands.DeleteBudget;

public class DeleteBudgetCommandValidator : AbstractValidator<DeleteBudgetCommand>
{
    public DeleteBudgetCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Budget Id cannot be empty!");
    }
}
