using ExpenseTrackerApi.Application.Budgets.Dtos;
using ExpenseTrackerApi.Application.Categories.Dtos;
using ExpenseTrackerApi.Application.Common.Interfaces;
using ExpenseTrackerApi.Application.Common.Mappings;
using ExpenseTrackerApi.Application.Expenses.Dtos;
using ExpenseTrackerApi.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerApi.Application.Expenses.Queries.ExportExpenses;

public class ExportExpensesQueryHandler : IRequestHandler<ExportExpensesQuery, byte[]>
{
    private readonly IAppDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;
    private readonly ICsvExportService _csvExportService;

    public ExportExpensesQueryHandler(IAppDbContext dbContext, ICurrentUserService currentUserService, ICsvExportService csvExportService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
        _csvExportService = csvExportService;
    }

    public async Task<byte[]> Handle(ExportExpensesQuery request, CancellationToken cancellationToken)
    {
        switch (request.ExportType)
        {
            case CsvExportType.Expenses:
                var expensesExport = await ExportExpenses(request, cancellationToken);
                return expensesExport;
            case CsvExportType.Budgets:
                var budgetsExport = await ExportBudgets(request, cancellationToken);
                return budgetsExport;
            case CsvExportType.Categories:
                var categoriesExport = await ExportCategories(request, cancellationToken);
                return categoriesExport;
            case CsvExportType.Complex:
                break;
            default:
                return [];
        }

        throw new NotImplementedException();
    }

    private async Task<byte[]> ExportExpenses(ExportExpensesQuery request, CancellationToken cancellationToken)
    {
        var q = _dbContext.Expenses
            .Include(x => x.Category)
            .AsNoTrackingWithIdentityResolution()
            .Where(x => x.UserId == _currentUserService.UserId);
        if (request.FromDate is not null)
        {
            q = q.Where(x => x.Date >= request.FromDate);
        }
        if (request.ToDate is not null)
        {
            q = q.Where(x => x.Date <= request.ToDate);
        }
        if (request.CategoryId is not null)
        {
            q = q.Where(x => x.CategoryId == request.CategoryId);
        }
        var expenses = await q.ToListAsync(cancellationToken);

        var expenseDtos = new List<ExpenseDto>();
        foreach (var item in expenses)
        {
            expenseDtos.Add(ExpenseMappingExtension.ToDto(item));
        }

        return _csvExportService.GenerateExpensesCsv(expenseDtos);
    }

    private async Task<byte[]> ExportBudgets(ExportExpensesQuery request, CancellationToken cancellationToken)
    {
        var q = _dbContext.Budgets
            .AsNoTrackingWithIdentityResolution()
            .Where(x => x.UserId == _currentUserService.UserId);

        if (request.CategoryId is not null)
        {
            q = q.Where(x => x.CategoryId == request.CategoryId);
        }

        var expenses = await q.ToListAsync(cancellationToken);

        var budgetDtos = new List<BudgetDto>();
        foreach (var item in expenses)
        {
            budgetDtos.Add(BudgetMappingExtension.ToDto(item));
        }

        return _csvExportService.GenerateBudgetsCsv(budgetDtos);
    }

    private async Task<byte[]> ExportCategories(ExportExpensesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _dbContext.Categories
            .AsNoTrackingWithIdentityResolution()
            .Where(x => x.UserId == _currentUserService.UserId)
            .ToListAsync(cancellationToken);

        var categoryDtos = new List<CategoryDto>();
        foreach (var item in categories)
        {
            categoryDtos.Add(CategoryMappingExtensions.ToDto(item));
        }

        return _csvExportService.GenerateCategoriesCsv(categoryDtos);
    }
}
