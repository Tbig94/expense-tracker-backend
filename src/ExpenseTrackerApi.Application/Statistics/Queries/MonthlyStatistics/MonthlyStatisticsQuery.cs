using ExpenseTrackerApi.Application.Budgets.Dtos;
using ExpenseTrackerApi.Application.Statistics.Dtos;
using MediatR;

namespace ExpenseTrackerApi.Application.Statistics.Queries.MonthlyStatistics;

public record MonthlyStatisticsQuery(MonthlyStatisticsRequest Dto) : IRequest<MonthlyStatisticsDto>;
