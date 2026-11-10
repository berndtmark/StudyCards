namespace StudyCards.Application.Common;

public enum ErrorType
{
    Generic,
    NotFound,
    Validation,
    Existing
}

public sealed record Failure(string Message, ErrorType Type = ErrorType.Generic);

public union Result<T>(T, Failure);
