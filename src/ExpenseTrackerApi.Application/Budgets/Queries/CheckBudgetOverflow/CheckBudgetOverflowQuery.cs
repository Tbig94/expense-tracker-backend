using ExpenseTrackerApi.Application.Budgets.Dtos;
using MediatR;

namespace ExpenseTrackerApi.Application.Budgets.Queries.CheckBudgetOVerflow;

public record CheckBudgetOverflowQuery(Guid CategoryId) : IRequest<BudgetStatusDto>;
