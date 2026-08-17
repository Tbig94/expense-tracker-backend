using ExpenseTrackerApi.Application.Expenses.Dtos;
using MediatR;

namespace ExpenseTrackerApi.Application.Expenses.Commands.UpdateExpense;

public record UpdateExpenseCommand(ExpenseDto Dto) : IRequest;
