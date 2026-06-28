namespace StudyCards.Api.Configuration.SecretsConfiguration.Builders;

public abstract class ConfigurationSecretBuilder
{
    public abstract string SecretKey { get; }
    public abstract IEnumerable<KeyValuePair<string, string>> WithConfiguration(string secret);
}