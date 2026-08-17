using ExpenseTrackerApi.Application.Categories.Dtos;
using MediatR;

namespace ExpenseTrackerApi.Application.Categories.Queries.GetCategories;

public record GetCategoriesQuery() : IRequest<List<CategoryDto>>;
