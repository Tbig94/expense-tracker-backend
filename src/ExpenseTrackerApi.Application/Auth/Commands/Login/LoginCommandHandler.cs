using ExpenseTrackerApi.Application.Auth.Dtos;
using ExpenseTrackerApi.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace ExpenseTrackerApi.Application.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResultDto>
{
    private readonly IAppDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;
    private readonly ITokenService _tokenService;


    public LoginCommandHandler(IAppDbContext dbContext, ICurrentUserService currentUserService, ITokenService tokenService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
        _tokenService = tokenService;
    }

    public async Task<LoginResultDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        Validate(request.User);
        var loginUser = request.User;
        string hashedPassword = _currentUserService.HashPassword(loginUser.Password);

        var user = await _dbContext.Users.FirstOrDefaultAsync(x =>
            string.Equals(x.Email, loginUser.Email));

        if (!BCrypt.Net.BCrypt.Verify(loginUser.Password, user.PasswordHash))
        {
            throw new Exception("Nem található felhasználó!");
        }

        var token = _tokenService.GenerateAccessToken(user.Id, user.Email);

        var result = new LoginResultDto
        {
            Email = user.Email,
            UserId = user.Id,
            Token = token
        };

        return result;
    }

    private void Validate(LoginDto user)
    {
        if (string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password))
        {
            throw new Exception("Üres felhasználó vagy jelszó!");
        }
    }
}
