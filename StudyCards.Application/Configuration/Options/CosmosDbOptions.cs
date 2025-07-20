namespace StudyCards.Application.Configuration.Options;

public class CosmosDbOptions
{
    public static string Key => "CosmosDb";

    public string? ConnectionMode { get; set; } = string.Empty;
    public bool? LimitToEndpoint { get; set; } = false;
}
