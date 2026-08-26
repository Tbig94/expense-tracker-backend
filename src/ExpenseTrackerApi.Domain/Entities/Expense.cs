using ExpenseTrackerApi.Domain.Common;

namespace ExpenseTrackerApi.Domain.Entities;

public class Expense : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid CategoryId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = default!;
    public DateTime Date { get; set; }

    public User User { get; set; } = default!;
    public Category Category { get; set; } = default!;

    public Expense() { }

    public Expense(Guid userId, Guid categoryId, decimal amount, string description, DateTime date)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        CategoryId = categoryId;
        Amount = amount;
        Description = description;
        Date = date;
        CreatedAt = DateTime.UtcNow;
    }
}
