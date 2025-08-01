namespace StudyCards.Application.Common;

public class Result<T>
{
    public bool IsSuccess => Data is not null && ErrorMessage is null;
    public string? ErrorMessage { get; }
    public T? Data { get; }

    protected Result(T? data, string? errorMessage = null)
    {
        Data = data;
        ErrorMessage = errorMessage;
    }

    public static Result<T> Success(T data) => new(data);
    public static Result<T> Failure(string errorMessage) => new(default, errorMessage);
    public static implicit operator Result<T>(T data) => new(data);
}
