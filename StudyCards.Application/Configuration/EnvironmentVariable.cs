namespace StudyCards.Application.Configuration;

public static class EnvironmentVariable
{
    public static string? SecretsApiKey => Environment.GetEnvironmentVariable("SECRETS_APIKEY");
}
