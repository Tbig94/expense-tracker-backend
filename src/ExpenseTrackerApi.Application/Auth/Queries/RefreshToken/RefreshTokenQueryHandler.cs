using ExpenseTrackerApi.Application.Auth.Dtos;
using ExpenseTrackerApi.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerApi.Application.Auth.Queries.RefreshToken;

public class RefreshTokenQueryHandler : IRequestHandler<RefreshTokenQuery, AuthResponse>
{
    private readonly IAppDbContext _dbContext;
    private readonly ITokenService _tokenService;

    public RefreshTokenQueryHandler(
        IAppDbContext dbContext,
        ITokenService tokenService)
    {
        _dbContext = dbContext;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> Handle(RefreshTokenQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            throw new ValidationException("Refresh token is required");
        }

        // 1. User keresése (Include-al beolvassuk a UserRoles-t)
        var user = await _dbContext.Users
            .AsNoTrackingWithIdentityResolution()
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken, cancellationToken);

        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid refresh token");
        }

        // 2. Token lejáratának ellenőrzése
        if (user.RefreshTokenExpiresAt < DateTime.UtcNow)
        {
            throw new UnauthorizedAccessException("Refresh token has expired");
        }

        // 3. Roles kinyerése a UserRoles-ből
        var roles = user.UserRoles
            .Select(ur => ur.Role.Name)
            .ToList();

        // 4. Új access token generálása
        var accessToken = _tokenService.GenerateAccessToken(user.Id, user.Email, roles);

        // 5. Új refresh token (Token Rotation)
        var newRefreshToken = _tokenService.GenerateRefreshToken();
        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7);

        await _dbContext.SaveChangesAsync(cancellationToken);

        // 6. Response
        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(15)
        };
    }
}
