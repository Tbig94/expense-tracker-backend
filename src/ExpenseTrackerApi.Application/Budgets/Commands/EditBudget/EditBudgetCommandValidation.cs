using FluentValidation;

namespace ExpenseTrackerApi.Application.Budgets.Commands.EditBudget;

public class EditBudgetCommandValidation : AbstractValidator<EditBudgetCommand>
{
    public EditBudgetCommandValidation()
    {
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage("Budget DTO cannot be null!");

        RuleFor(x => x.Dto.Id)
            .NotEmpty()
            .WithMessage("Id cannot be empty!");
    }
}
