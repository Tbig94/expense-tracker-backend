namespace ExpenseTrackerApi.Application.Statistics.Dtos;

public class MonthlyStatisticsDto
{
    public List<CategoryStatisticsDto> CategoryStats { get; set; } = [];
}

public class CategoryStatisticsDto
{
    public Guid? CategoryId { get; set; }
    public Guid? UserId { get; set; }
    public decimal? Limit { get; set; }
    public decimal? Amount { get; set; }
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