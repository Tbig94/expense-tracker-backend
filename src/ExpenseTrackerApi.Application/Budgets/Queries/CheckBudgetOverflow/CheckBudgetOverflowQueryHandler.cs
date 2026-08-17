using ExpenseTrackerApi.Application.Budgets.Dtos;
using ExpenseTrackerApi.Application.Common;
using ExpenseTrackerApi.Application.Common.Interfaces;
using ExpenseTrackerApi.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerApi.Application.Budgets.Queries.CheckBudgetOVerflow;

public class CheckBudgetOverflowQueryHandler : IRequestHandler<CheckBudgetOverflowQuery, BudgetStatusDto>
{
    private readonly IAppDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public CheckBudgetOverflowQueryHandler(IAppDbContext dbContext, ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task<BudgetStatusDto> Handle(CheckBudgetOverflowQuery request, CancellationToken cancellationToken)
    {
        var budget = await _dbContext.Budgets.FirstOrDefaultAsync(x => 
            x.UserId == _currentUserService.UserId && 
            x.CategoryId == request.CategoryId, 
            cancellationToken) ??
            throw new NotFoundException("Budget not found!");


        var expense = await _dbContext.Expenses.FirstOrDefaultAsync(x => 
            x.UserId == _currentUserService.UserId && 
            x.CategoryId == request.CategoryId, 
            cancellationToken) ??
            throw new NotFoundException("Expense not found!");

        var budgetState = BudgetStateCalculator.Calculate(budget.LimitAmount, expense.Amount);
        var budgetStatus = new BudgetStatusDto(budget.LimitAmount, expense.Amount, budget.LimitAmount - expense.Amount, budgetState);

        return budgetStatus;
    }
}
