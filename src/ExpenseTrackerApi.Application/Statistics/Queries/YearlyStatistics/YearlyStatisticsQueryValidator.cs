using FluentValidation;

namespace ExpenseTrackerApi.Application.Statistics.Queries.YearlyStatistics;

public class YearlyStatisticsQueryValidator : AbstractValidator<YearlyStatisticsQuery>
{
    public YearlyStatisticsQueryValidator()
    {
        //RuleFor(x => x.Dto)
        //    .NotNull()
        //    .WithMessage("Statistics Dto cannot be null!");

        //RuleFor(x => x.Dto.Year)
        //    .LessThanOrEqualTo(DateTime.UtcNow.Year)
        //    .WithMessage("Year cannot be null!");
    }
}
