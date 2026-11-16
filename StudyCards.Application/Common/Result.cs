namespace StudyCards.Application.Common;

public class Result<T>
{
    public bool IsSuccess => ErrorMessageType == null;
    public string? ErrorMessage { get; }
    public ErrorType? ErrorMessageType { get; }
    public T? Data { get; }

    protected Result(T? data, string? errorMessage = null, ErrorType? errorType = null)
    {
        Data = data;
        ErrorMessage = errorMessage;
        ErrorMessageType = errorType;
    }

    public static Result<T> Success(T data) => new(data);
    public static Result<T> Failure(string errorMessage) => new(default, errorMessage, ErrorType.Generic);
    public static Result<T> Failure(string errorMessage, ErrorType errorType) => new(default, errorMessage, errorType);
    public static implicit operator Result<T>(T data) => new(data);
}

public enum ErrorType
{
    Generic,
    NotFound,
    Validation,
    Existing
}
