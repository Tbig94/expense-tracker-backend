using ExpenseTrackerApi.Application.Common;
using ExpenseTrackerApi.Application.Common.Interfaces;
using ExpenseTrackerApi.Application.Dashboard.Dtos;
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
        var expenses = _dbContext.Expenses
            .Include(x => x.Category)
            .AsNoTrackingWithIdentityResolution()
            .Where(x => x.Date.Year == request.Dto.Year &&
                        x.Date.Month == request.Dto.Month &&
                        x.UserId == _currentUser.UserId);

        var expensesGrouped = expenses
            .GroupBy(x => x.Category);

        var budgets = await _dbContext.Budgets
            .AsNoTrackingWithIdentityResolution()
            .Where(b => b.UserId == _currentUser.UserId &&
                        b.ValidTo.Month == request.Dto.Month &&
                        b.ValidTo.Year == request.Dto.Year)
            .Select(b => new
            {
                b.LimitAmount,
                b.Category.Name,
                b.Category.Color,
                SpentAmount = _dbContext.Expenses
                    .Where(e => e.UserId == _currentUser.UserId &&
                                e.CategoryId == b.CategoryId &&
                                e.Date.Month == request.Dto.Month &&
                                e.Date.Year == request.Dto.Year)
                    .Sum(e => (decimal?)e.Amount) ?? 0m
            })
            .ToListAsync(cancellationToken);

        var monthlyStatDto = new MonthlyStatisticsDto();


        var daysInMonth = DateTime.DaysInMonth(request.Dto.Year, request.Dto.Month);
        var totalSpendings = expenses.Select(x => x.Amount).Sum();
        monthlyStatDto.TotalSpendings = totalSpendings;
        monthlyStatDto.AverageDailySpending = totalSpendings / daysInMonth;
        monthlyStatDto.NumberOfActiveBudgets = budgets.Count();
        monthlyStatDto.NumberOfTransactions = expenses.Count();

        var categoryBreakdowns = await _dbContext.Expenses
            .AsNoTrackingWithIdentityResolution()
            .Where(x => x.UserId == _currentUser.UserId &&
                        x.Date.Year == request.Dto.Year &&
                        x.Date.Month == request.Dto.Month)
            .GroupBy(x => new { x.Category.Name, x.Category.Color })
            .Select(g => new CategoryBreakdownDto
            {
                CategoryName = g.Key.Name,
                CategoryColor = g.Key.Color,
                Amount = g.Sum(x => x.Amount),
                Percentage = totalSpendings == 0
                    ? 0
                    : g.Sum(x => x.Amount) / totalSpendings * 100
            })
            .ToListAsync(cancellationToken);

        monthlyStatDto.CategoryBreakdowns = categoryBreakdowns;



        monthlyStatDto.Budgets = budgets
        .Select(b => new BudgetDashboardDto
        {
            CategoryName = b.Name,
            CategoryColor = b.Color,
            LimitAmount = b.LimitAmount.Value,
            SpentAmount = b.SpentAmount,
            RemainingAmount = b.LimitAmount.Value - b.SpentAmount,
            State = BudgetStateCalculator.Calculate(b.LimitAmount, b.SpentAmount)
        })
        .ToList();

        return monthlyStatDto;
    }
}
