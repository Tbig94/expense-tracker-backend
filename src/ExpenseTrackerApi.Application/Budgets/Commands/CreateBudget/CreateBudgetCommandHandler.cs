using ExpenseTrackerApi.Application.Common.Interfaces;
using ExpenseTrackerApi.Application.Common.Mappings;
using ExpenseTrackerApi.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerApi.Application.Budgets.Commands.CreateBudget;

public class CreateBudgetCommandHandler : IRequestHandler<CreateBudgetCommand>
{
    private readonly IAppDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public CreateBudgetCommandHandler(IAppDbContext dbContect, ICurrentUserService currentUserService)
    {
        _dbContext = dbContect;
        _currentUserService = currentUserService;
    }

    public async Task Handle(CreateBudgetCommand request, CancellationToken cancellationToken)
    {
        var categoryExists = _dbContext.Categories
            .FirstOrDefault(c => c.Id == request.Budget.CategoryId) ??
            throw new NotFoundException($"Category with this ID not found!");

        var currentUserId = _currentUserService.UserId;
        var categoryBelongsToUser = _dbContext.Categories
            .FirstOrDefault(
                c => c.Id == request.Budget.CategoryId && (c.UserId == currentUserId ||c.UserId == null)) ??
                throw new UnauthorizedAccessException(
                    $"User does not have access to this category!");

        var existingBudget = await _dbContext.Budgets
            .FirstOrDefaultAsync(
                b => b.CategoryId == request.Budget.CategoryId &&
                     b.UserId == currentUserId &&
                     b.ValidTo >= DateTime.Now, cancellationToken);
        if (existingBudget is not null)
        {
            throw new InvalidOperationException(
                    $"The budget for this category already exists for this period."); 
        }

        if (request.Budget.LimitAmount <= 0)
        {
            throw new ArgumentException("Limit amount must be greater than zero");
        }

        var budget = BudgetMappingExtension.ToEntity(request.Budget);
        budget.UserId = currentUserId;

        await _dbContext.Budgets.AddAsync(budget, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
