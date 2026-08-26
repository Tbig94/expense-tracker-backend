using MediatR;

namespace ExpenseTrackerApi.Application.Categories.Commands.DeleteCategory;

public record DeleteCategoryCommand(Guid Id) : IRequest;
