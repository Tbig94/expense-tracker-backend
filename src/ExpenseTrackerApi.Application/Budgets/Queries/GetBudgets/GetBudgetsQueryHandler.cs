using ExpenseTrackerApi.Application.Budgets.Dtos;
using ExpenseTrackerApi.Application.Common.Interfaces;
using ExpenseTrackerApi.Application.Common.Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerApi.Application.Budgets.Queries.GetBudgets;

public class GetBudgetsQueryHandler : IRequestHandler<GetBudgetsQuery, List<BudgetDto>>
{
    private readonly IAppDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public GetBudgetsQueryHandler(IAppDbContext dbContext, ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task<List<BudgetDto>> Handle(GetBudgetsQuery request, CancellationToken cancellationToken)
    {
        var budgets = await _dbContext.Budgets
            .Where(x => 
                x.UserId == _currentUserService.UserId &&
                x.ValidTo.Month == DateTime.Now.Month)
            .AsNoTrackingWithIdentityResolution()
            .ToListAsync(cancellationToken);

        var budgetDtos = new List<BudgetDto>();
        foreach (var budget in budgets)
        {
            budgetDtos.Add(BudgetMappingExtension.ToDto(budget));
        }

        return budgetDtos;
    }
}
