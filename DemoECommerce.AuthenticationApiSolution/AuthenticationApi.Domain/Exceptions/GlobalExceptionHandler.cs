using AuthenticationApi.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ProductApi.Domain.Exceptions;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "An error occurred at {Path}! Message: {Message}",
            httpContext.Request.Path, exception.Message);

        var (status, title) = exception switch
        {
            UserNotFoundException => (StatusCodes.Status404NotFound, "Not Found"),
            EmailAlreadyExistsException => (StatusCodes.Status409Conflict, "Conflict"),
            InvalidCredentialsException => (StatusCodes.Status401Unauthorized, "Unauthorized"),
            _ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
        };

        httpContext.Response.StatusCode = status;
        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = exception.Message
        }, cancellationToken);

        return true;
    }
}
