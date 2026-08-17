using ExpenseTrackerApi.Domain.Common;

namespace ExpenseTrackerApi.Domain.Entities;

public class Budget : BaseEntity
{
    public Guid UserId { get; set; }

    public Guid CategoryId { get; set; }

    public decimal? LimitAmount { get; set; }

    public int Month { get; set; }

    public int Year { get; set; }

    public User? User { get; set; }
    public Category? Category { get; set; }


    public Budget(Guid userId, Guid categoryId, decimal? limitAmount, int month, int year)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        CategoryId = categoryId;
        LimitAmount = limitAmount;
        Month = month;
        Year = year;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateLimit(decimal newLimit)
    {
        LimitAmount = newLimit;
        UpdatedAt = DateTime.UtcNow;
    }
}
