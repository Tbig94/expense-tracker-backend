using ExpenseTrackerApi.Application.Expenses.Dtos;
using MediatR;

namespace ExpenseTrackerApi.Application.Expenses.Queries.GetAllExpenses;

public record GetAllExpensesQuery : IRequest<List<ExpenseDto>>;
