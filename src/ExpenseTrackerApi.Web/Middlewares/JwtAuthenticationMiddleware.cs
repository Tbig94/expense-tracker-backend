using ExpenseTrackerApi.Application.Common.Interfaces;
using System.Security.Claims;

namespace ExpenseTrackerApi.Web.Middlewares;

public class JwtAuthenticationMiddleware
{
    private readonly RequestDelegate _next;

    public JwtAuthenticationMiddleware(RequestDelegate next)
    { 
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, 
                                  ITokenService tokenService, 
                                  ICurrentUserService currentUserService,
                                  IConfiguration configuration)
    {
        try
        {
            // Header lekérdeuése, validálása:
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

            if (string.IsNullOrEmpty(authHeader))
            {
                await _next(context);
                return;
            }

            if (!authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new { error = "Invalid authorization header format" });
                return;
            }

            // Token lekérdezése, validálása
            var token = authHeader.Substring("Bearer ".Length).Trim();

            var validationResult = tokenService.ValidateToken(token);

            if (!validationResult.IsValid)
            {
                await context.Response.WriteAsJsonAsync(new { error = validationResult.Error });
                return;
            }

            // Claim-ek decode-olása, kinyerése
            var claims = tokenService.DecodeToken(token);

            if (!claims.TryGetValue(ClaimTypes.NameIdentifier, out var userIdStr) || 
                !Guid.TryParse(userIdStr, out var userId))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new { error = "Invalid token!" });
                return;
            }

            var email = claims.TryGetValue(ClaimTypes.Email, out var emailValue) ? emailValue : null;
            var roles = claims.TryGetValue(ClaimTypes.Role, out var rolesStr) ? rolesStr.Split(',').ToList() : [];

            // ICurrentUserService feltöltése
            currentUserService.SetCurrentUser(userId, email, roles);

            // HttpContext.User beállítása (Authorization header-ből)
            var claimsIdentity = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, email ?? string.Empty)
            ], "Jwt");

            // Roles hozzáadása
            foreach (var role in roles)
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            context.User = new ClaimsPrincipal(claimsIdentity);


            await _next(context);
        }
        catch (Exception ex)
        {
            // A GlobalExceptionHandler ezt majd elfogja
            throw;
        }

    }
}
