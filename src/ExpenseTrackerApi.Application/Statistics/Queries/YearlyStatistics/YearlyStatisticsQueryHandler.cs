using ExpenseTrackerApi.Application.Common.Interfaces;
using ExpenseTrackerApi.Application.Statistics.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerApi.Application.Statistics.Queries.YearlyStatistics;

public class YearlyStatisticsQueryHandler : IRequestHandler<YearlyStatisticsQuery, YearlyStatisticsDto>
{
    private readonly IAppDbContext _dbContext;
    private readonly ICurrentUserService _currentUser;

    public YearlyStatisticsQueryHandler(IAppDbContext dbContext, ICurrentUserService currentUser)
    {
        _dbContext = dbContext;
        _currentUser = currentUser;
    }

    public async Task<YearlyStatisticsDto> Handle(YearlyStatisticsQuery request, CancellationToken cancellationToken)
    {
        var yearlyStat = new YearlyStatisticsDto();

        var now = DateTime.UtcNow;
        var currentYear = now.Year;
        var currentMonth = now.Month;

        var monthlySpendings = await _dbContext.Expenses
            .Where(x => x.UserId == _currentUser.UserId &&
                        x.Date.Year == currentYear)
            .GroupBy(x => x.Date.Month)
            .Select(g => new MonthlySpendingTrend
            {
                Month = g.Key,
                Amount = g.Sum(x => x.Amount)
            })
            .ToListAsync(cancellationToken);

        yearlyStat.TopCategories = await _dbContext.Expenses
            .Include(x => x.Category)
            .Where(x => x.UserId == _currentUser.UserId &&
                        x.Date.Year == currentYear)
            .GroupBy(x => x.Category.Name)
            .OrderByDescending(g => g.Sum(x => x.Amount))
            .Take(5)
            .Select(g => new CategorySpending
            {
                Category = g.Key,
                Amount = g.Sum(x => x.Amount)
            })
            .ToListAsync(cancellationToken);

        for (int i = 1; i <= 12; i++)
        {
            if (monthlySpendings.Any(x => x.Month == i))
            {
                yearlyStat.MonthlySpendingTrend.Add(monthlySpendings.First(x => x.Month == i));
            }
            else
            {
                yearlyStat.MonthlySpendingTrend.Add(new MonthlySpendingTrend { Amount = 0, Month = i });
            }
        }
        //yearlyStat.MonthlySpendingTrend = monthlySpendings;

        decimal totalThisYear = _dbContext.Expenses
            .Where(x => x.UserId == _currentUser.UserId &&
                        x.Date.Year == currentYear)
            .Select(x => x.Amount)
            .Sum(x => x);

        decimal monthlyAverage = totalThisYear / (decimal)12;

        yearlyStat.TotalThisYear = totalThisYear;
        yearlyStat.MonthlyAverage = monthlyAverage;

        yearlyStat.PeakMonth = monthlySpendings
            .OrderByDescending(x => x.Amount)
            .Take(1)
            .FirstOrDefault();

        yearlyStat.QuietestMonth = monthlySpendings
            .OrderBy(x => x.Amount)
            .Take(1)
            .FirstOrDefault();

        return yearlyStat;
    }
}
