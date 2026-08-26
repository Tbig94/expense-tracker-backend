using ExpenseTrackerApi.Application.Common.Interfaces;
using ExpenseTrackerApi.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerApi.Application.Budgets.Commands.DeleteBudget;

public class DeleteBudgetCommandHandler : IRequestHandler<DeleteBudgetCommand>
{
    private readonly IAppDbContext _dbContext;

    public DeleteBudgetCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(DeleteBudgetCommand request, CancellationToken cancellationToken)
    {
        var budget = await _dbContext.Budgets.FirstOrDefaultAsync(x => 
            x.Id == request.Id, cancellationToken) ??
            throw new NotFoundException($"Budget not found!");

        _dbContext.Budgets.Attach(budget);
        _dbContext.Budgets.Remove(budget);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
