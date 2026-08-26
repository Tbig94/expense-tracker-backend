using ExpenseTrackerApi.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ExpenseTrackerApi.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private Guid? _userId;
    private string? _email;
    private List<string>? _roles;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId
    {
        get
        {
            if (_userId.HasValue)
            {
                return _userId.Value;
            }

            var context = _httpContextAccessor.HttpContext;
            if (context?.User == null)
                return new Guid();

            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdClaim, out var userId))
            {
                _userId = userId;
                return userId;
            }

            return new Guid();
        }
    }

    public string Email
    {
        get
        {
            if (!string.IsNullOrEmpty(_email))
            {
                return _email;
            }

            var context = _httpContextAccessor.HttpContext;
            if (context?.User == null)
                return null;

            _email = context.User.FindFirst(ClaimTypes.Email)?.Value;
            return _email;
        }
    }

    public List<string> Roles
    {
        get
        {
            if (_roles != null)
            {
                return _roles;
            }

            var context = _httpContextAccessor.HttpContext;
            if (context?.User == null)
                return new List<string>();

            _roles = context.User
                .FindAll(ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            return _roles;
        }
    }

    public void SetCurrentUser(Guid userId, string email, List<string> roles)
    {
        _userId = userId;
        _email = email;
        _roles = roles ?? new List<string>();
    }

    public string HashPassword(string rawPassword)
    {
        return BCrypt.Net.BCrypt.HashPassword(rawPassword, workFactor: 10);
    }

    public bool VerifyPassword(string rawPassword, string storedHashFromDb)
    {
        return BCrypt.Net.BCrypt.Verify(rawPassword, storedHashFromDb);
    }
}
