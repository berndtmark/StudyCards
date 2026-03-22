namespace StudyCards.Api.Configuration.Options;

public class OpenTelemetryOptions
{
    public string Endpoint { get; set; } = string.Empty;
    public string Headers { get; set; } = string.Empty;
    public string ServiceName { get; set; } = string.Empty;
}
