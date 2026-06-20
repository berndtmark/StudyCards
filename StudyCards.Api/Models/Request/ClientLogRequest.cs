namespace StudyCards.Api.Models.Request;

public class ClientLogRequest
{
    public required string Message { get; init; }
    public string? StackTrace { get; init; }
    public string? Source { get; init; }
    public string? Path { get; init; }

    public override string ToString()
    {
        return $"Client Error: {Message} | Source: {Source} | Path: {Path} | StackTrace: {StackTrace}";
    }
}
