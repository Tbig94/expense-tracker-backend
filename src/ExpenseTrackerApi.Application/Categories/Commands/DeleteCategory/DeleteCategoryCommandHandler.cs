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
        if (budgets.Count > 0) throw new InvalidOperationException("Category has budgets!");

        var expenses = await _dbContext.Expenses
            .AsNoTrackingWithIdentityResolution()
            .Where(x => x.CategoryId == category.Id)
            .ToListAsync(cancellationToken);
        if (expenses.Count > 0) throw new InvalidOperationException("Category has expenses!");

        _dbContext.Categories.Attach(category);
        _dbContext.Categories.Remove(category);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
