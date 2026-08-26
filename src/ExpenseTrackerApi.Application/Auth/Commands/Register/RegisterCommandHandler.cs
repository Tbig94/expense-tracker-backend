using ExpenseTrackerApi.Application.Auth.Dtos;
using ExpenseTrackerApi.Application.Common.Interfaces;
using ExpenseTrackerApi.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerApi.Application.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand>
{
    private readonly IAppDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public RegisterCommandHandler(IAppDbContext dbContext, ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var newUser = request.User;

        await Validate(newUser);

        string hashedPassword = _currentUserService.HashPassword(newUser.Password);
        var user = new User()
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            Email = newUser.Email,
            Name = newUser.Name,
            PasswordHash = hashedPassword,
            UpdatedAt = DateTime.UtcNow,
        };
        await _dbContext.Users.AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task Validate(RegisterDto newUser)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => string.Equals(x.Email, newUser.Email));
        if (user is not null)
        {
            throw new Exception("Már létezik a felhasználó!");
        }
    }
}
