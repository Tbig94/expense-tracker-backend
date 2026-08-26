namespace ExpenseTrackerApi.Application.Categories.Dtos;

public record CategoryDto(Guid Id, Guid? UserId, string Name, string Color);