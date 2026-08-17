using ExpenseTrackerApi.Application.Expenses.Dtos;
using ExpenseTrackerApi.Domain.Entities;

namespace ExpenseTrackerApi.Application.Common.Mappings;

public static class ExpenseMappingExtension
{
    public static ExpenseDto ToDto(this Expense entity)
    {
        var categoryDto = CategoryMappingExtensions.ToDto(entity.Category);
        var dto = new ExpenseDto
        {
            Id = entity.Id,
            Date = entity.Date,
            CategoryId = entity.CategoryId,
            CategoryName = entity.Category.Name,
            Description = entity.Description,
            Amount = entity.Amount
        };
        return dto;
    }

    public static Expense ToEntity(this ExpenseDto dto)
    {


        var entity = new Expense(dto.UserId.Value, dto.CategoryId!.Value, dto.Amount, dto.Description, dto.Date!.Value) {  };
        return entity;
    }
}
