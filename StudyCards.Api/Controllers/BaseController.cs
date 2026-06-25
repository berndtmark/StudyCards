using Microsoft.AspNetCore.Mvc;
using StudyCards.Application.Common;

namespace StudyCards.Api.Controllers;

public abstract class BaseController : ControllerBase
{
    public virtual IActionResult HandleResult<T>(Result<T> result)
    {
        return result.IsSuccess
         ? Ok(result.Data)
         : HandleError(result);
    }

    public virtual IActionResult HandleResult<TData, TResponse>(Result<TData> result, Func<TData, TResponse> mapper)
    {
        return result.IsSuccess
            ? Ok(mapper(result.Data!))
            : HandleError(result);
    }

    private ObjectResult HandleError<T>(Result<T> result)
    {
        var (statusCode, title) = result.ErrorMessageType switch
        {
            ErrorType.Existing => (StatusCodes.Status409Conflict, "Data Conflict"),
            ErrorType.NotFound => (StatusCodes.Status404NotFound, "Resource not found"),
            ErrorType.Validation => (StatusCodes.Status400BadRequest, "Validation error"),
            _ => (StatusCodes.Status500InternalServerError, "An error has occurred")
        };

        return Problem(
            statusCode: statusCode,
            title: title,
            detail: result.ErrorMessage
        );
    }
}
