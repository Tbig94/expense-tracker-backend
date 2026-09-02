using ExpenseTrackerApi.Application.Auth.Dtos;
using ExpenseTrackerApi.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerApi.Application.Auth.Queries.GetAccountInfo;

public class GetAccountInfoQueryHandler : IRequestHandler<GetAccountInfoQuery, AccountDto>
{
    private readonly IAppDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public GetAccountInfoQueryHandler(IAppDbContext dbContext, ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task<AccountDto?> Handle(GetAccountInfoQuery request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .AsNoTrackingWithIdentityResolution()
            .FirstOrDefaultAsync(x =>
                x.Id == _currentUserService.UserId, cancellationToken);

        if (user is not null)
        {
            var result = new AccountDto()
            {
                Email = user.Email,
                Name = user.Name
            };
            return result;
        }
        else
        {
            return null;
        }
    }
}
