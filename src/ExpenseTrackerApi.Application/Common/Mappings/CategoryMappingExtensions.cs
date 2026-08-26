using ExpenseTrackerApi.Application.Categories.Dtos;
using ExpenseTrackerApi.Domain.Entities;
using System.Drawing;
using System.Xml.Linq;

namespace ExpenseTrackerApi.Application.Common.Mappings;

public static class CategoryMappingExtensions
{
    public static CategoryDto ToDto(this Category entity)
        => new(entity.Id, entity.UserId, entity.Name, entity.Color);

    public static Category ToEntity(this CategoryDto dto)
        //=> new(dto.Id, dto.UserId, dto.Name, dto.Color);
        => new (dto.UserId, dto.Name, dto.Color);
}
