using ExpenseTrackerApi.Application.Budgets.Dtos;
using ExpenseTrackerApi.Application.Statistics.Dtos;
using ExpenseTrackerApi.Application.Statistics.Queries.MonthlyStatistics;
using ExpenseTrackerApi.Application.Statistics.Queries.YearlyStatistics;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerApi.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StatisticsController : ControllerBase
{
    private readonly IMediator _mediator;

    public StatisticsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize]
    [HttpGet(nameof(GetMonthlyStatistics))]
    public async Task<MonthlyStatisticsDto> GetMonthlyStatistics([FromQuery] MonthlyStatisticsRequest dto)
    {
        var result = await _mediator.Send(new MonthlyStatisticsQuery(dto));

        return result;
    }

    [Authorize]
    [HttpGet(nameof(GetYearlyStatistics))]
    public async Task<YearlyStatisticsDto> GetYearlyStatistics([FromQuery] YearlyStatisticsRequest dto)
    {
        var result = await _mediator.Send(new YearlyStatisticsQuery(dto));

        return result;
    }
}
