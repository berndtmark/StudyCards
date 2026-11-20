using Microsoft.AspNetCore.Mvc;
using StudyCards.Application.Common;

namespace StudyCards.Api.Controllers;

public abstract class BaseController : ControllerBase
{
    public virtual IActionResult HandleResult<T>(Result<T> result) =>
        result switch
        {
            T data => Ok(data),
            Failure failure => HandleError(failure)
        };

    public virtual IActionResult HandleResult<TData, TResponse>(Result<TData> result, Func<TData, TResponse> mapper) =>
        result switch
        {
            TData data => Ok(mapper(data)),
            Failure failure => HandleError(failure)
        };

    private ObjectResult HandleError(Failure failure)
    {
        var (statusCode, title) = failure.Type switch
        {
            ErrorType.Existing => (StatusCodes.Status409Conflict, "Data Conflict"),
            ErrorType.NotFound => (StatusCodes.Status404NotFound, "Resource not found"),
            ErrorType.Validation => (StatusCodes.Status400BadRequest, "Validation error"),
            _ => (StatusCodes.Status500InternalServerError, "An error has occurred")
        };

        return Problem(statusCode: statusCode, title: title, detail: failure.Message);
    }
}
