namespace ExpenseTrackerApi.Application.Expenses.Dtos;

public record ExpenseDto
{
    public Guid? Id { get; set; }

    public Guid? CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public Guid? UserId { get; set; }

    public DateTime? Date { get; set; }

    public decimal Amount { get; set; }

    public string? Description { get; set; }
};

public record CreateExpenseDto
{
    public Guid CategoryId { get; set; }

    public DateTime Date { get; set; }

    public decimal Amount { get; set; }

    public string Description { get; set; }

}
