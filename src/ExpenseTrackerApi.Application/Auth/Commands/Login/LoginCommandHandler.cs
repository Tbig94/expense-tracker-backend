using ExpenseTrackerApi.Application.Auth.Dtos;
using ExpenseTrackerApi.Application.Common.Interfaces;
using ExpenseTrackerApi.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerApi.Application.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IAppDbContext _dbContext;
    private readonly ITokenService _tokenService;


    public LoginCommandHandler(IAppDbContext dbContext, ITokenService tokenService)
    {
        _dbContext = dbContext;
        _tokenService = tokenService;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        Validate(request.User);
        var loginUser = request.User;

        var user = await _dbContext.Users.FirstOrDefaultAsync(x =>
            string.Equals(x.Email, loginUser.Email), cancellationToken) ??
            throw new NotFoundException("Cannot find user!");

        if (!BCrypt.Net.BCrypt.Verify(loginUser.Password, user.PasswordHash))
        {
            throw new NotFoundException("Cannot find user!");
        }

        var token = _tokenService.GenerateAccessToken(user.Id, user.Email);

        return new LoginResponse
        {
            UserDto = new LoginResultDto
            {
                Email = user.Email,
                UserId = user.Id
            },
            AccessToken = token
        };
    }

    private static void Validate(LoginDto user)
    {
        if (string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password))
        {
            throw new InvalidOperationException("Empty email or password!");
        }
    }
}
