using FluentValidation;

namespace ExpenseTrackerApi.Application.Expenses.Commands.DeleteExpense;

public class DeleteExpenseCommandValidator : AbstractValidator<DeleteExpenseCommand>
{
    public DeleteExpenseCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Expense Id cannot be empty!");
    }
}
