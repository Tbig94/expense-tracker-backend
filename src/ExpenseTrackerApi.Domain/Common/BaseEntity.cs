namespace ExpenseTrackerApi.Domain.Common;

public class BaseEntity : IAuditable
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
