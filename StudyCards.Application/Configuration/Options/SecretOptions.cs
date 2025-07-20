namespace StudyCards.Application.Configuration.Options;

public class SecretOptions
{
    public const string Key = "Secrets";

    public string ApiKey { get; set; } = string.Empty;
    public string OrganizationId { get; set; } = string.Empty;
    public string ApiUrl { get; set; } = string.Empty;
    public string IdentityUrl { get; set; } = string.Empty;
}
