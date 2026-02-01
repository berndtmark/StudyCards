using Microsoft.AspNetCore.Mvc;
using StudyCards.Application.Common;

namespace StudyCards.Api.Controllers;

public abstract class BaseController : ControllerBase
{
    public virtual IActionResult HandleError<T>(Result<T> result)
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
