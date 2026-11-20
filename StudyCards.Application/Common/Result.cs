namespace StudyCards.Application.Common;

public enum ErrorType
{
    Generic,
    NotFound,
    Validation,
    Existing
}

public sealed record Failure(string Message, ErrorType Type = ErrorType.Generic);

public readonly union Result<T>(T, Failure)
{
    // A helper to get the result directly from a usecase without needing a switch
    public readonly T Data => Value switch
    {
        T t => t,
        Failure f => throw new InvalidOperationException($"Cannot access Data when Result is Failure: {f.Type}: {f.Message}")
    };

    public readonly bool IsSuccess => Value is T;
}
