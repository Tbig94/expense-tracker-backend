using FluentValidation;

namespace ExpenseTrackerApi.Application.Statistics.Queries.MonthlyStatistics;

public class MonthlyStatisticsQueryValidator : AbstractValidator<MonthlyStatisticsQuery>
{
    public MonthlyStatisticsQueryValidator()
    {
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage("Statistics Dto cannot be null!");

        RuleFor(x => x.Dto.Year)
            .LessThanOrEqualTo(DateTime.UtcNow.Year)
            .WithMessage("Year cannot be null!");

        RuleFor(x => x.Dto.Month)
            .InclusiveBetween(1, 12)
            .WithMessage("Month cannot be null!");
    }
}
