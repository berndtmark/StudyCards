using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using StudyCards.Application.Exceptions;

namespace StudyCards.Api.Configuration.ExceptionHandlers;

internal sealed class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService, ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var statusCode = exception switch
        {
            EntityNotFoundException => StatusCodes.Status404NotFound,
            ValidationException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        httpContext.Response.StatusCode = statusCode;
        Log(statusCode, exception);

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Type = exception.GetType().Name,
                Title = "An error occured",
                Detail = exception.Message
            }
        });
    }

    private void Log(int statusCode, Exception exception)
    {
        if (statusCode >= StatusCodes.Status500InternalServerError)
        {
            logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);
        }
        else
        {
            logger.LogWarning(exception, "An exception occurred: {Message}", exception.Message);
        }
    }
}
