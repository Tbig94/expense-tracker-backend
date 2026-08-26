using ExpenseTrackerApi.Application.Budgets.Dtos;
using MediatR;

namespace ExpenseTrackerApi.Application.Budgets.Commands.CreateBudget;

public record CreateBudgetCommand(CreateBudgetDto Budget) : IRequest;

