using ExpenseTrackerApi.Domain.Enums;
using MediatR;

namespace ExpenseTrackerApi.Application.Expenses.Queries.ExportExpenses;

public record ExportExpensesQuery(DateTime? FromDate,
                                  DateTime? ToDate,
                                  Guid? CategoryId,
                                  CsvExportType ExportType) : IRequest<byte[]>;
