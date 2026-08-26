using ExpenseTrackerApi.Domain.Common;

namespace ExpenseTrackerApi.Domain.Entities;

public class Role : BaseEntity
{
    public string? Name { get; set; }
    public string? Description { get; set; }

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
