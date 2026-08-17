using ExpenseTrackerApi.Application.Budgets.Dtos;
using ExpenseTrackerApi.Application.Categories.Dtos;

namespace ExpenseTrackerApi.Application.Expenses.Dtos;

public class ExpenseExportDto
{
    public List<ExpenseDto> Expenses { get; set; } = [];

    public List<CategoryDto> Categories { get; set; } = [];

    public List<BudgetDto> Budgets { get; set; } = [];
}
