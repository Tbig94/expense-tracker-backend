using ExpenseTrackerApi.Domain.Common;

namespace ExpenseTrackerApi.Domain.Entities;

public class Category : BaseEntity
{
    public Guid? UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Color { get; set; } = null!;

    public bool IsDefault { get; set; }

    public ICollection<Expense> Expenses { get; } = [];


    public User? User { get; set; }

    public Category(Guid? userId, string name, string color)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Name = name;
        Color = color;
        IsDefault = false;
        CreatedAt = DateTime.UtcNow;
    }
}
