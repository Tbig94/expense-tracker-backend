using ExpenseTrackerApi.Application.Categories.Commands.CreateCategory;
using ExpenseTrackerApi.Application.Categories.Commands.DeleteCategory;
using ExpenseTrackerApi.Application.Categories.Dtos;
using ExpenseTrackerApi.Application.Categories.Queries.GetCategories;
using ExpenseTrackerApi.Application.Categories.Queries.GetCategory;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerApi.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoryController(IMediator mediator) => _mediator = mediator;

    [Authorize]
    [HttpGet(nameof(GetAll))]
    public async Task<IActionResult> GetAll()
        => Ok(await _mediator.Send(new GetCategoriesQuery()));

    [Authorize]
    [HttpGet("{Id}")]
    public async Task<IActionResult> GetById(Guid id)
        => Ok(await _mediator.Send(new GetCategoryQuery(id)));

    [Authorize]
    [HttpPost(nameof(Create))]
    public async Task<IActionResult> Create(CreateCategoryDto dto)
    {
        await _mediator.Send(new CreateCategoryCommand(dto.Name, dto.Color));
        return NoContent();
    }

    [Authorize]
    [HttpDelete(nameof(Delete))]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteCategoryCommand(id));
        return NoContent();
    }
}
