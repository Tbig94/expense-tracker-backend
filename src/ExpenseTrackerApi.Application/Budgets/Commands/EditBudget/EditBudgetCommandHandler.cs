using ExpenseTrackerApi.Application.Common.Interfaces;
using ExpenseTrackerApi.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerApi.Application.Budgets.Commands.EditBudget;

public class EditBudgetCommandHandler : IRequestHandler<EditBudgetCommand>
{
    private readonly IAppDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public EditBudgetCommandHandler(IAppDbContext dbContext, ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task Handle(EditBudgetCommand request, CancellationToken cancellationToken)
    {
        var budget = await _dbContext.Budgets.FirstOrDefaultAsync(x =>
            x.UserId == _currentUserService.UserId &&
            x.Id == request.Dto.Id, cancellationToken) ??
            throw new NotFoundException($"Budget with this category does not exists!");

        budget.LimitAmount = request.Dto.LimitAmount!.Value;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
