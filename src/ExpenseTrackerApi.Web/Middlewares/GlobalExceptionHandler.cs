using ExpenseTrackerApi.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerApi.Web.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An error occurred");

        var response = new ProblemDetails
        {
            Instance = httpContext.Request.Path,
        };

        httpContext.Response.ContentType = "application/json";

        if (exception is NotFoundException notFoundEx)
        {
            httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            response.Title = "Not Found";
            response.Status = StatusCodes.Status404NotFound;
            response.Detail = notFoundEx.Message;
        }
        else if (exception is UnauthorizedAccessException unauthorizedEx)
        {
            httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
            response.Title = "Forbidden";
            response.Status = StatusCodes.Status403Forbidden;
            response.Detail = unauthorizedEx.Message;
        }
        else if (exception is DuplicateBudgetException duplicateEx)
        {
            httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
            response.Title = "Conflict";
            response.Status = StatusCodes.Status409Conflict;
            response.Detail = duplicateEx.Message;
        }
        else if (exception is InvalidOperationException invalidOpEx)
        {
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            response.Title = "Bad Request";
            response.Status = StatusCodes.Status400BadRequest;
            response.Detail = invalidOpEx.Message;
        }
        else if (exception is ArgumentException argEx)
        {
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            response.Title = "Bad Request";
            response.Status = StatusCodes.Status400BadRequest;
            response.Detail = argEx.Message;
        }
        else
        {
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            response.Title = "Internal Server Error";
            response.Status = StatusCodes.Status500InternalServerError;
            response.Detail = "An unexpected error occurred. Please try again later.";
        }

        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
        return true;
    }
}
