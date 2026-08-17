using ExpenseTrackerApi.Domain.Entities;
using ExpenseTrackerApi.Domain.Enums;

namespace ExpenseTrackerApi.Application.Common;

public static class BudgetStateCalculator
{
    public static BudgetState Calculate(decimal? limitAmount, decimal spentAmount)
    {
        if (spentAmount == 0)
            return BudgetState.Empty;

        if (spentAmount > limitAmount)
            return BudgetState.Exceeded;

        if (spentAmount >= limitAmount * 0.8m)
            return BudgetState.NearLimit;

        return BudgetState.Normal;
    }
}
