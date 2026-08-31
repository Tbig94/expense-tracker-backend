using ExpenseTrackerApi.Application.Common.Interfaces;
using ExpenseTrackerApi.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerApi.Application.Auth.Commands.DeleteAccount;

public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand>
{
    private readonly IAppDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public DeleteAccountCommandHandler(IAppDbContext dbContext, ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x =>
            x.Id == _currentUserService.UserId, cancellationToken) ??
            throw new NotFoundException("Cannot find user!");

        var userExpenses = await _dbContext.Expenses.Where(x =>
            x.UserId == _currentUserService.UserId)
            .ToListAsync(cancellationToken);

        var userBudgets = await _dbContext.Budgets.Where(x =>
            x.UserId == _currentUserService.UserId)
            .ToListAsync(cancellationToken);

        var userCategories = await _dbContext.Categories.Where(x =>
            x.UserId == _currentUserService.UserId)
            .ToListAsync(cancellationToken);

        _dbContext.Expenses.AttachRange(userExpenses);
        _dbContext.Budgets.AttachRange(userBudgets);
        _dbContext.Categories.AttachRange(userCategories);
        _dbContext.Users.Attach(user);
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
