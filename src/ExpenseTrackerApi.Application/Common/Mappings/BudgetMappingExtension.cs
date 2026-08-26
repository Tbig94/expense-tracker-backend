using ExpenseTrackerApi.Application.Budgets.Dtos;
using ExpenseTrackerApi.Domain.Entities;

namespace ExpenseTrackerApi.Application.Common.Mappings;

public static class BudgetMappingExtension
{
    public static BudgetDto ToDto(this Budget entity)
        => new(entity.UserId, entity.Id, entity.CategoryId, entity.LimitAmount, entity.Month, entity.Year);

    public static Budget ToEntity(this BudgetDto dto)
        => new(dto.UserId, dto.CategoryId, dto.LimitAmount, dto.Month, dto.Year);

    public static Budget ToEntity(this CreateBudgetDto dto)
        => new(Guid.NewGuid(), dto.CategoryId, dto.LimitAmount, dto.Month, dto.Year);
}
