using ExpenseTrackerApi.Domain.Enums;

namespace ExpenseTrackerApi.Application.Budgets.Dtos;

public record BudgetDto(
    Guid UserId,
    Guid Id,
    Guid CategoryId,
    decimal? LimitAmount,
    DateTime ValidFrom,
    DateTime ValidTo);

public record MonthlyStatisticsRequest(
    int Month,
    int Year);


public record CreateBudgetDto(
    Guid CategoryId,
    decimal LimitAmount);


public record EditBudgetDto(
    Guid Id,
    decimal? LimitAmount);

public record BudgetStatusDto(
    decimal? LimitAmount,
    decimal? SpentAmount,
    decimal? RemainingAmount,
    BudgetState State
    );