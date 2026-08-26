using ExpenseTrackerApi.Application.Budgets.Dtos;
using MediatR;

namespace ExpenseTrackerApi.Application.Budgets.Queries.GetBudgets;

public record GetBudgetsQuery() : IRequest<List<BudgetDto>>;
