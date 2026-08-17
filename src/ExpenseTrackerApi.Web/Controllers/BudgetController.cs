using ExpenseTrackerApi.Application.Budgets.Commands.CreateBudget;
using ExpenseTrackerApi.Application.Budgets.Commands.DeleteBudget;
using ExpenseTrackerApi.Application.Budgets.Commands.EditBudget;
using ExpenseTrackerApi.Application.Budgets.Dtos;
using ExpenseTrackerApi.Application.Budgets.Queries.CheckBudgetOVerflow;
using ExpenseTrackerApi.Application.Budgets.Queries.GetBudgets;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerApi.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BudgetController : ControllerBase
{
    private readonly IMediator _mediator;

    public BudgetController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize]
    [HttpGet(nameof(GetAll))]
    public async Task<IActionResult> GetAll()
        => Ok(await _mediator.Send(new GetBudgetsQuery()));


    [Authorize]
    [HttpPost(nameof(Create))]
    public async Task<IActionResult> Create(CreateBudgetDto budget)
    {
        await _mediator.Send(new CreateBudgetCommand(budget));

        return NoContent();
    }

    [Authorize]
    [HttpDelete(nameof(Delete))]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteBudgetCommand(id));

        return NoContent();
    }

    [Authorize]
    [HttpPut(nameof(Edit))]
    public async Task<IActionResult> Edit(EditBudgetDto budget)
    {
        await _mediator.Send(new EditBudgetCommand(budget));

        return NoContent();
    }

    [Authorize]
    [HttpGet(nameof(CheckBudgetOverflow))]
    public async Task<IActionResult> CheckBudgetOverflow(Guid categoryId)
        => Ok(await _mediator.Send(new CheckBudgetOverflowQuery(categoryId)));
}
