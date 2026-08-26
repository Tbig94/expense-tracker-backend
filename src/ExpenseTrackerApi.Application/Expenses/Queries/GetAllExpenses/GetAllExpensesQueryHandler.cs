using ExpenseTrackerApi.Application.Common.Interfaces;
using ExpenseTrackerApi.Application.Common.Mappings;
using ExpenseTrackerApi.Application.Expenses.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerApi.Application.Expenses.Queries.GetAllExpenses;

public class GetAllExpensesQueryHandler : IRequestHandler<GetAllExpensesQuery, List<ExpenseDto>>
{
    private readonly IAppDbContext _dbContext;
    private readonly ICurrentUserService _currentUser;

    public GetAllExpensesQueryHandler(IAppDbContext dbContext, ICurrentUserService currentUser)
    {
        _dbContext = dbContext;
        _currentUser = currentUser;
    }

    public async Task<List<ExpenseDto>> Handle(GetAllExpensesQuery request, CancellationToken cancellationToken)
    {
        var expenses = await _dbContext.Expenses
            .Where(x => x.UserId == _currentUser.UserId)
            .Include(x => x.Category)
            .OrderByDescending(x => x.Date)
            .ToListAsync(cancellationToken);
        var expenseDtos = new List<ExpenseDto>();
        foreach (var expense in expenses)
        {
            var expenseDto = ExpenseMappingExtension.ToDto(expense);
            expenseDto.CategoryName = expense.Category.Name;
            expenseDtos.Add(expenseDto);
        }

        return expenseDtos;
    }
}
