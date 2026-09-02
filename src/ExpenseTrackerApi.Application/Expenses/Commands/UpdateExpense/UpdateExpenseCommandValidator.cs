using FluentValidation;

namespace ExpenseTrackerApi.Application.Expenses.Commands.UpdateExpense;

public class UpdateExpenseCommandValidator : AbstractValidator<UpdateExpenseCommand>
{
    public UpdateExpenseCommandValidator()
    {
        RuleFor(x => x.Dto.Id)
            .NotEmpty()
            .WithMessage("Expense Id cannot be empty");

        RuleFor(x => x.Dto.UserId)
           .NotEmpty()
           .WithMessage("Expense UserId cannot be empty");
    }
}
