using ExpenseTrackerApi.Application.Expenses.Dtos;
using MediatR;

namespace ExpenseTrackerApi.Application.Expenses.Commands.CreateExpense;

public record CreateExpenseCommand(CreateExpenseDto Expense) : IRequest;