using ExpenseTrackerApi.Application.Common.Interfaces;
using MediatR;

namespace ExpenseTrackerApi.Application.Auth.Commands.Logout;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Unit>
{
    private readonly IAppDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public LogoutCommandHandler(
        IAppDbContext dbContext,
        ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException("User not found");
        }

        var user = await _dbContext.Users.FindAsync(new object[] { userId }, cancellationToken: cancellationToken);

        if (user == null)
        {
            throw new UnauthorizedAccessException("User not found");
        }

        // Refresh token nullázása
        user.RefreshToken = null;
        user.RefreshTokenExpiresAt = null;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
