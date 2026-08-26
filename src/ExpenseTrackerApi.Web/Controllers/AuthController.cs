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
    public async Task<IActionResult> Login(LoginDto user)
    {
        var result = await _mediator.Send(new LoginCommand(user));
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost(nameof(Logout))]
    public async Task<IActionResult> Logout()
    {
        var command = new LogoutCommand();
        await _mediator.Send(command);
        return Ok(new { message = "Logged out successfully" });
    }

    [AllowAnonymous]
    [HttpPost(nameof(RefreshToken))]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var query = new RefreshTokenQuery { RefreshToken = request.RefreshToken };
        var result = await _mediator.Send(query);
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
        var query = new GetAccountInfoQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
