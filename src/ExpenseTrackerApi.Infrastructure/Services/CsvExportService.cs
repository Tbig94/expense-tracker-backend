using CsvHelper;
using ExpenseTrackerApi.Application.Budgets.Dtos;
using ExpenseTrackerApi.Application.Categories.Dtos;
using ExpenseTrackerApi.Application.Common.Interfaces;
using ExpenseTrackerApi.Application.Dashboard.Dtos;
using ExpenseTrackerApi.Application.Expenses.Dtos;
using System.Globalization;
using System.Text;

namespace ExpenseTrackerApi.Infrastructure.Services;

public class CsvExportService : ICsvExportService
{
    public byte[] GenerateBudgetsCsv(List<BudgetDto> budgets)
    {
        using (var memoryStream = new MemoryStream())
        using (var writer = new StreamWriter(memoryStream, Encoding.UTF8))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(budgets);
            writer.Flush();
            return memoryStream.ToArray();
        }
    }

    public byte[] GenerateCategoriesCsv(List<CategoryDto> categories)
    {
        using (var memoryStream = new MemoryStream())
        using (var writer = new StreamWriter(memoryStream, Encoding.UTF8))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(categories);
            writer.Flush();
            return memoryStream.ToArray();
        }
    }

    public byte[] GenerateComplexReportCsv(DashboardDto dashboard)
    {
        using (var memoryStream = new MemoryStream())
        using (var writer = new StreamWriter(memoryStream, Encoding.UTF8))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecord(dashboard);
            writer.Flush();
            return memoryStream.ToArray();
        }
    }

    public byte[] GenerateExpensesCsv(List<ExpenseDto> expenses)
    {
        using (var memoryStream = new MemoryStream())
        using (var writer = new StreamWriter(memoryStream, Encoding.UTF8))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(expenses);
            writer.Flush();
            return memoryStream.ToArray();
        }
    }
}
