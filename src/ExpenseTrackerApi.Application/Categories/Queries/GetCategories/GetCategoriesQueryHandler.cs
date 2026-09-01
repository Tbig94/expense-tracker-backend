using ExpenseTrackerApi.Application.Categories.Dtos;
using ExpenseTrackerApi.Application.Common.Interfaces;
using ExpenseTrackerApi.Application.Common.Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerApi.Application.Categories.Queries.GetCategories;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, List<CategoryDto>>
{
    private readonly IAppDbContext _dbContext;
    private readonly ICurrentUserService _currentUser;

    public GetCategoriesQueryHandler(IAppDbContext dbContext, ICurrentUserService currentUser)
    {
        _dbContext = dbContext;
        _currentUser = currentUser;
    }

    public async Task<List<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = _dbContext.Categories
            .Include(x => x.Expenses)
            .Include(x => x.Budgets)
            .Where(x => (x.UserId == _currentUser.UserId && !x.IsDefault) ||
                        x.IsDefault)
            .AsNoTrackingWithIdentityResolution();

        var dtos = new List<CategoryDto>();
        foreach (var category in categories)
        {
            var categoryDto = CategoryMappingExtensions.ToDto(category);
            categoryDto.HasExpense = (category.Expenses.Count > 0 || category.Budgets.Count > 0) ? true : false;
            dtos.Add(categoryDto);
        }

        return dtos;
    }
}
        