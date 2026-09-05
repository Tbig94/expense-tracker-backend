using ExpenseTrackerApi.Application.Dashboard.Dtos;

namespace ExpenseTrackerApi.Application.Statistics.Dtos;

public class MonthlyStatisticsDto
{
    // small cards: Total Spending, active categories, avg daily spending, transactions

    public List<BudgetDashboardDto> Budgets { get; set; } = [];

    public List<CategoryBreakdownDto> CategoryBreakdowns { get; set; } = [];

    public decimal TotalSpendings { get; set; } //ok

    public decimal AverageDailySpending { get; set; } //ok

    public int NumberOfActiveBudgets { get; set; }

    public int NumberOfTransactions { get; set; }
}

public class YearlyStatisticsDto()
{
    public decimal TotalThisYear { get; set; }

    public decimal MonthlyAverage { get; set; }

    public MonthlySpendingTrend? PeakMonth { get; set; }

    public MonthlySpendingTrend? QuietestMonth { get; set; }

    public List<MonthlySpendingTrend> MonthlySpendingTrend { get; set; } = [];

    public List<CategorySpending> TopCategories { get; set; } = [];
}

public class MonthlySpendingTrend()
{
    public decimal Amount { get; set; }

    public int Month { get; set; }
}


public class CategorySpending()
{
    public decimal Amount { get; set; }

    public string? Category { get; set; }
}