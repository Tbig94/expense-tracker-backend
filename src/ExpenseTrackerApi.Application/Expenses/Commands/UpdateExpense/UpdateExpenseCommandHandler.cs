using ExpenseTrackerApi.Application.Common.Interfaces;
using ExpenseTrackerApi.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerApi.Application.Expenses.Commands.UpdateExpense;

public class UpdateExpenseCommandHandler : IRequestHandler<UpdateExpenseCommand>
{
    private readonly IAppDbContext _dbContext;

    public UpdateExpenseCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = await _dbContext.Expenses
            .FirstOrDefaultAsync(x => x.Id == request.Dto.Id) ??
            throw new NotFoundException("Expense not found!");

        if (expense is not null)
        {
            expense.Description = request.Dto.Description!;
            expense.CategoryId = request.Dto.CategoryId!.Value;
            expense.Date = request.Dto.Date!.Value;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
