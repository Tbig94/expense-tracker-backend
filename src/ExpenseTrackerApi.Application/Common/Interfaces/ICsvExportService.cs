using ExpenseTrackerApi.Application.Budgets.Dtos;
using ExpenseTrackerApi.Application.Categories.Dtos;
using ExpenseTrackerApi.Application.Dashboard.Dtos;
using ExpenseTrackerApi.Application.Expenses.Dtos;

namespace ExpenseTrackerApi.Application.Common.Interfaces;

public interface ICsvExportService
{
    byte[] GenerateExpensesCsv(List<ExpenseDto> expenses);

    byte[] GenerateCategoriesCsv(List<CategoryDto> categories);

    byte[] GenerateBudgetsCsv(List<BudgetDto> budgets);

    byte[] GenerateComplexReportCsv(DashboardDto dashboard);
}
