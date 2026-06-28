using Microsoft.Extensions.Configuration;

namespace StudyCards.Api.Configuration.SecretsConfiguration.Builders;

public class PrimitiveSecretBuilder(string secretKey, string configKey, IConfiguration configuration) : ConfigurationSecretBuilder
{
    public override string SecretKey => secretKey;

    public override IEnumerable<KeyValuePair<string, string>> WithConfiguration(string secret)
    {
        var existingValue = configuration[configKey];
        var finalValue = string.IsNullOrEmpty(existingValue) ? secret : existingValue;

        yield return new KeyValuePair<string, string>(configKey, finalValue);
    }
}
