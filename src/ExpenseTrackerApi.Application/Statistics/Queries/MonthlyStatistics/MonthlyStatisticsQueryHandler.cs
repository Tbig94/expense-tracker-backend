using ExpenseTrackerApi.Application.Common.Interfaces;
using ExpenseTrackerApi.Application.Statistics.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerApi.Application.Statistics.Queries.MonthlyStatistics;

public class MonthlyStatisticsQueryHandler : IRequestHandler<MonthlyStatisticsQuery, MonthlyStatisticsDto>
{
    private readonly IAppDbContext _dbContext;
    private readonly ICurrentUserService _currentUser;

    public MonthlyStatisticsQueryHandler(IAppDbContext dbContext, ICurrentUserService currentUser)
    {
        _dbContext = dbContext;
        _currentUser = currentUser;
    }

    public async Task<MonthlyStatisticsDto> Handle(MonthlyStatisticsQuery request, CancellationToken cancellationToken)
    {
        var expenses = await _dbContext.Expenses
            .Include(x => x.Category)
            .Where(x => x.Date.Year == request.Dto.Year &&
                        x.Date.Month == request.Dto.Month &&
                        x.UserId == _currentUser.UserId)
            .GroupBy(x => x.Category)
            .ToListAsync(cancellationToken);

        var budgets = await _dbContext.Budgets
            .Include(x => x.Category)
            .Where(x => x.Month == request.Dto.Month &&
                        x.Year == request.Dto.Year &&
                        x.UserId == _currentUser.UserId)
            .ToListAsync(cancellationToken);

        var monthlyStat = new MonthlyStatisticsDto();

        foreach (var item in expenses)
        {
            monthlyStat.CategoryStats.Add(new CategoryStatisticsDto()
            {
                CategoryId = item.Key.Id,
                //Limit = request.Dto.LimitAmount,
                Limit = budgets.First(x => x.CategoryId == item.Key.Id).LimitAmount,
                UserId = item.Key.UserId,
                Amount = item.Key.Expenses.Select(x => x.Amount).Sum()
            });
        }

        return monthlyStat;
    }
}
