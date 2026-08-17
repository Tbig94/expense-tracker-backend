namespace ExpenseTrackerApi.Application.Expenses.Dtos;

public class GetExpensesFilterDto
{
    public string? Text { get; set; }

    public DateTime? MinDate { get; set; }

    public DateTime? MaxDate { get; set; }

    public decimal? MinAmount { get; set; }

    public decimal? MaxAmount { get; set; }

    public string? CategoryName { get; set; }
}
