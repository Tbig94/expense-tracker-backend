using ExpenseTrackerApi.Application.Expenses.Dtos;
using MediatR;

namespace ExpenseTrackerApi.Application.Expenses.Queries.GetExpensesByFilter;

public record GetExpensesByFilterQuery(GetExpensesFilterDto Filter) : IRequest<List<ExpenseDto>>;
