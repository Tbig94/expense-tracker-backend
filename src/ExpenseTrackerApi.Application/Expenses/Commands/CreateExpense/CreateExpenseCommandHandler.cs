using ExpenseTrackerApi.Application.Common.Interfaces;
using ExpenseTrackerApi.Domain.Entities;
using MediatR;

namespace ExpenseTrackerApi.Application.Expenses.Commands.CreateExpense;

public class CreateExpenseCommandHandler : IRequestHandler<CreateExpenseCommand>
{
    private readonly IAppDbContext _dbContext;
    private readonly ICurrentUserService _currentUser;

    public CreateExpenseCommandHandler(IAppDbContext dbContext, ICurrentUserService currentUser)
    {
        _dbContext = dbContext;
        _currentUser = currentUser;
    }

    public async Task Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = new Expense(_currentUser.UserId, 
                                  request.Expense.CategoryId,
                                  request.Expense.Amount,
                                  request.Expense.Description, 
                                  request.Expense.Date);

        await _dbContext.Expenses.AddAsync(expense, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
