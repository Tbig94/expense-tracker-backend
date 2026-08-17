using ExpenseTrackerApi.Domain.Enums;

namespace ExpenseTrackerApi.Application.Dashboard.Dtos;

public class DashboardDto
{
    public CurrentMonth CurrentMonth { get; set; } = new();

    public MonthlySummary MonthlySummary { get; set; } = new();

    public List<CategoryBreakdownDto> CategoryBreakdowns { get; set; } = [];

    public List<BudgetDashboardDto> Budgets { get; set; } = [];

    public List<Expense> TopExpenses { get; set; } = [];

    public List<BudgetDashboardDto> BudgetWarnings { get; set; } = [];

    public int NumberOfTransactions { get; set; }
}

public class CurrentMonth
{
    public int Year { get; set; }

    public int Month { get; set; }
}

public class MonthlySummary
{
    public decimal TotalAmount { get; set; }

    public decimal PreviousMonthAmount { get; set; }

    public decimal ChangePercent { get; set; }
}

public class CategoryBreakdownDto
{
    public string CategoryName { get; set; }

    public string CategoryColor { get; set; }

    public decimal Amount { get; set; }

    public decimal Percentage { get; set; }
}

public class BudgetDashboardDto
{
    public string CategoryName { get; set; }

    public string CategoryColor{ get; set; }

    public decimal LimitAmount { get; set; }

    public decimal SpentAmount { get; set; }

    public decimal RemainingAmount { get; set; }

    public BudgetState State { get; set; }
}

public class Expense
{
    public string Description { get; set; }

    public decimal Amount { get; set; }

    public DateTime Date { get; set; }

    public string CategoryName { get; set; }
}