using ExpenseTrackerApi.Domain.Enums;

namespace ExpenseTrackerApi.Application.Expenses.Dtos;

public class ExportFilterDto
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get ; set; }
    public Guid? CategoryId { get; set; }
    public CsvExportType ExportType { get; set; }
}
