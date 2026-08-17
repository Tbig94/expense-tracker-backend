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
    public List<CategoryStatisticsDto> CategoryStats { get; set; } = [];
}
