using ExpenseTrackerApi.Application.Budgets.Dtos;
using ExpenseTrackerApi.Domain.Entities;

namespace ExpenseTrackerApi.Application.Common.Mappings;

public static class BudgetMappingExtension
{
    public static BudgetDto ToDto(this Budget entity)
        => new(entity.UserId, entity.Id, entity.CategoryId, entity.LimitAmount, entity.ValidFrom, entity.ValidTo);

    public static Budget ToEntity(this BudgetDto dto)
        => new(dto.UserId, dto.CategoryId, dto.LimitAmount);

    public static Budget ToEntity(this CreateBudgetDto dto)
        => new(Guid.NewGuid(), dto.CategoryId, dto.LimitAmount);
}
