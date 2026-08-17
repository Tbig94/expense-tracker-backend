using ExpenseTrackerApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerApi.Application.Common.Interfaces;

public interface IAppDbContext
{
    DbSet<Category> Categories { get; }
    DbSet<Expense> Expenses { get; }
    DbSet<User> Users { get; }
    DbSet<Budget> Budgets { get; }


    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    int SaveChanges();
}