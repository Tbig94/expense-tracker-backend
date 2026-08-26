using ExpenseTrackerApi.Application.Budgets.Dtos;
using MediatR;

namespace ExpenseTrackerApi.Application.Budgets.Commands.EditBudget;

public record EditBudgetCommand(EditBudgetDto Dto) : IRequest;
