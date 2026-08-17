namespace ExpenseTrackerApi.Web.Middlewares;

using System.Security.Claims;

public class DevelopmentAuthenticationMiddleware
{
    private readonly RequestDelegate _next;

    public DevelopmentAuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.User.Identity?.IsAuthenticated ?? true)
        {
            // Dummy user hozzáadása Development-ben
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "dev-user-id"),
                new Claim(ClaimTypes.Name, "Developer"),
                new Claim(ClaimTypes.Email, "dev@local.com")
            };

            var identity = new ClaimsIdentity(claims, "DevelopmentScheme");
            var principal = new ClaimsPrincipal(identity);
            context.User = principal;
        }

        await _next(context);
    }
}