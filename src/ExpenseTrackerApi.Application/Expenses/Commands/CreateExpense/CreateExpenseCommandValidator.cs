using FluentValidation;

namespace ExpenseTrackerApi.Application.Expenses.Commands.CreateExpense;

public class CreateExpenseCommandValidator : AbstractValidator<CreateExpenseCommand>
{
    public CreateExpenseCommandValidator()
    {
        RuleFor(x => x.Expense)
            .NotNull()
            .WithMessage("Expense cannot be null!");

        RuleFor(x => x.Expense.CategoryId)
            .NotEmpty()
            .WithMessage("Category Id cannot be empty!");

        RuleFor(x => x.Expense.Amount)
            .NotEmpty()
            .WithMessage("Expense amount cannot be empty!");

        RuleFor(x => x.Expense.Date)
            .NotEmpty()
            .WithMessage("Expense Date cannot be empty!");

        RuleFor(x => x.Expense.Description)
            .NotEmpty()
            .WithMessage("Expense Description cannot be empty!");
    }
}
