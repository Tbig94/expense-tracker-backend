using MediatR;

namespace ExpenseTrackerApi.Application.Budgets.Commands.DeleteBudget;

public record DeleteBudgetCommand(Guid Id) : IRequest;
