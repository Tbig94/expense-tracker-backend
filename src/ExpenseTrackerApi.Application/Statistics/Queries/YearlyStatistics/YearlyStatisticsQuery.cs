using ExpenseTrackerApi.Application.Budgets.Dtos;
using ExpenseTrackerApi.Application.Statistics.Dtos;
using MediatR;

namespace ExpenseTrackerApi.Application.Statistics.Queries.YearlyStatistics;

public record YearlyStatisticsQuery() : IRequest<YearlyStatisticsDto>;
