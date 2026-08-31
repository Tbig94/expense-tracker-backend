using ExpenseTrackerApi.Application.Auth.Dtos;
using ExpenseTrackerApi.Application.Common.Interfaces;
using ExpenseTrackerApi.Domain.Exceptions;
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

    public async Task<AccountDto> Handle(GetAccountInfoQuery request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x =>
            x.Id == _currentUserService.UserId, cancellationToken) ??
            throw new NotFoundException("Cannot find user!");

        var result = new AccountDto()
        {
            Email = user.Email,
            Name = user.Name
        };

        return result;
    }
}
