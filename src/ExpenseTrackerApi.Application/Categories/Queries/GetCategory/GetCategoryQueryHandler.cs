using ExpenseTrackerApi.Application.Categories.Dtos;
using ExpenseTrackerApi.Application.Common.Interfaces;
using ExpenseTrackerApi.Application.Common.Mappings;
using ExpenseTrackerApi.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerApi.Application.Categories.Queries.GetCategory;

public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, CategoryDto>
{
    private readonly IAppDbContext _dbContext;

    public GetCategoryQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CategoryDto> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        var category = await _dbContext.Categories
            .AsNoTrackingWithIdentityResolution()
            .FirstOrDefaultAsync(x => 
                x.Id == request.Id,
                cancellationToken) ??
                throw new NotFoundException("Category not found!");

        var categoryDto = CategoryMappingExtensions.ToDto(category);
        return categoryDto;
    }
}
