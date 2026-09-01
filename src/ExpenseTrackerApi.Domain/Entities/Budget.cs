using ExpenseTrackerApi.Domain.Common;

namespace ExpenseTrackerApi.Domain.Entities;

public class Budget : BaseEntity
{
    public Guid UserId { get; set; }

    public Guid CategoryId { get; set; }

    public decimal? LimitAmount { get; set; }

    public DateTime ValidFrom { get; set; }

    public DateTime ValidTo { get; set; }

    public User? User { get; set; }
    public Category? Category { get; set; }


    public Budget(Guid userId, Guid categoryId, decimal? limitAmount)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        CategoryId = categoryId;
        LimitAmount = limitAmount;
        ValidFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
        ValidTo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month), 23, 59, 59);
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateLimit(decimal newLimit)
    {
        LimitAmount = newLimit;
        UpdatedAt = DateTime.UtcNow;
    }
}
