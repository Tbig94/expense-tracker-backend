using ExpenseTrackerApi.Application.Expenses.Commands.CreateExpense;
using ExpenseTrackerApi.Application.Expenses.Commands.DeleteExpense;
using ExpenseTrackerApi.Application.Expenses.Commands.UpdateExpense;
using ExpenseTrackerApi.Application.Expenses.Dtos;
using ExpenseTrackerApi.Application.Expenses.Queries.ExportExpenses;
using ExpenseTrackerApi.Application.Expenses.Queries.GetAllExpenses;
using ExpenseTrackerApi.Application.Expenses.Queries.GetExpensesByFilter;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerApi.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExpenseController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExpenseController(IMediator mediator) => _mediator = mediator;

    [Authorize]
    [HttpGet(nameof(GetByFilter))]
    public async Task<IActionResult> GetByFilter([FromQuery] GetExpensesFilterDto dto)
        => Ok(await _mediator.Send(new GetExpensesByFilterQuery(dto)));

    [Authorize]
    [HttpGet(nameof(GetAll))]
    public async Task<IActionResult> GetAll()
        => Ok(await _mediator.Send(new GetAllExpensesQuery()));

    [Authorize]
    [HttpPost(nameof(Create))]
    public async Task<IActionResult> Create(CreateExpenseDto dto)
    {
        await _mediator.Send(new CreateExpenseCommand(dto));

        return NoContent();
    }

    [Authorize]
    [HttpDelete(nameof(Delete))]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteExpenseCommand(id));

        return NoContent();
    }

    [Authorize]
    [HttpPut(nameof(Update))]
    public async Task<IActionResult> Update(ExpenseDto dto)
    {
        await _mediator.Send(new UpdateExpenseCommand(dto));

        return NoContent();
    }

    [Authorize]
    [HttpGet(nameof(Export))]
    public async Task<IActionResult> Export([FromQuery] ExportFilterDto dto)
    {
        var export = await _mediator.Send(new ExportExpensesQuery(dto.FromDate, dto.ToDate, dto.CategoryId, dto.ExportType));

        return File(export, "text/csv; charset=utf-8", "export.csv");
    }
}
