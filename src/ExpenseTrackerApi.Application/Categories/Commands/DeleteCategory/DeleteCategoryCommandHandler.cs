using ExpenseTrackerApi.Application.Common.Interfaces;
using ExpenseTrackerApi.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerApi.Application.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
{
    private readonly IAppDbContext _dbContext;

    public DeleteCategoryCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _dbContext.Categories
            .AsNoTrackingWithIdentityResolution()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken) ??
            throw new NotFoundException("Category not found!");

        var budgets = await _dbContext.Budgets
            .AsNoTrackingWithIdentityResolution()
            .Where(x => x.CategoryId == category.Id)
            .ToListAsync(cancellationToken);
        var expenses = await _dbContext.Expenses
            .AsNoTrackingWithIdentityResolution()
            .Where(x => x.CategoryId == category.Id)
            .ToListAsync(cancellationToken);
        foreach (var budget in budgets)
        {
            _dbContext.Budgets.Attach(budget);
            _dbContext.Budgets.Remove(budget);
        }
        foreach (var expense in expenses)
        {
            _dbContext.Expenses.Attach(expense);
            _dbContext.Expenses.Remove(expense);
        }

        _dbContext.Categories.Attach(category);
        _dbContext.Categories.Remove(category);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
