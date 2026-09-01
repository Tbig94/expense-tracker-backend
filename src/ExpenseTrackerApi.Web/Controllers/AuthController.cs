using ExpenseTrackerApi.Application.Auth.Commands.DeleteAccount;
using ExpenseTrackerApi.Application.Auth.Commands.Login;
using ExpenseTrackerApi.Application.Auth.Commands.Logout;
using ExpenseTrackerApi.Application.Auth.Commands.Register;
using ExpenseTrackerApi.Application.Auth.Dtos;
using ExpenseTrackerApi.Application.Auth.Queries.GetAccountInfo;
using ExpenseTrackerApi.Application.Auth.Queries.RefreshToken;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerApi.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private const string USER_TOKEN = "userToken";

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpPost(nameof(Register))]
    public async Task<IActionResult> Register(RegisterDto user)
    {
        await _mediator.Send(new RegisterCommand(user));
        return NoContent();
    }

    [AllowAnonymous]
    [HttpPost(nameof(Login))]
    public async Task<IActionResult> Login([FromBody] LoginDto user)
    {
        var result = await _mediator.Send(new LoginCommand(user));

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTimeOffset.UtcNow.AddHours(1)
        };

        Response.Cookies.Append(USER_TOKEN, result.AccessToken, cookieOptions);

        return Ok(result.UserDto);
    }

    [AllowAnonymous]
    [HttpPost(nameof(Logout))]
    public async Task<IActionResult> Logout()
    {

        Response.Cookies.Delete(USER_TOKEN, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None
        });
        await _mediator.Send(new LogoutCommand());
        return Ok(new { message = "Logged out successfully" });
    }

    [AllowAnonymous]
    [HttpPost(nameof(RefreshToken))]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var result = await _mediator.Send(new RefreshTokenQuery { RefreshToken = request.RefreshToken });
        return Ok(result);
    }

    [Authorize]
    [HttpPost(nameof(DeleteAccount))]
    public async Task<IActionResult> DeleteAccount()
    {
        var command = new DeleteAccountCommand();
        await _mediator.Send(command);
        return NoContent();
    }

    [Authorize]
    [HttpGet(nameof(GetAccountInfo))]
    public async Task<IActionResult> GetAccountInfo()
    {
        var result = await _mediator.Send(new GetAccountInfoQuery());
        return Ok(result);
    }
}
