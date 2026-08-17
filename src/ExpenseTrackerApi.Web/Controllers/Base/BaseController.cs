using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerApi.Web.Controllers.Base;

[Route("api/[controller]")]
[ApiController]
public abstract class BaseController : ControllerBase
{
    protected IActionResult Success<T>(T data, string? message = null)
    {
        var response = ApiResponse<T>.Success(data, message);
        return Ok(response);
    }

    protected IActionResult Failure(string message, List<string>? errors = null, int statusCode = 400)
    {
        var response = ApiResponse<object>.Failure(errors ?? new List<string>(), message);
        return StatusCode(statusCode, response);
    }

    protected IActionResult NotFoundResponse(string message = "A kért erőforrás nem található.")
    {
        return Failure(message, statusCode: 404);
    }
}

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }

    public T? Data { get; set; }

    public string? Message { get; set; }

    public List<string>? Errors { get; set; }


    public static ApiResponse<T> Success(T data, string? message = null)
        => new() { IsSuccess = true, Data = data, Message = message };

    public static ApiResponse<T> Failure(List<string> errors, string? message = null)
        => new() { IsSuccess = false, Errors = errors, Message = message };
}