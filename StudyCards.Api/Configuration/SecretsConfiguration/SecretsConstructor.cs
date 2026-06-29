using StudyCards.Api.Configuration.SecretsConfiguration.Builders;
using StudyCards.Infrastructure.Secrets;

namespace StudyCards.Api.Configuration.SecretsConfiguration;

public class SecretsConstructor(IConfiguration configuration, IConfigurationProvider configurationProvider)
{
    public IEnumerable<ConfigurationSecretBuilder> Builders => [
        new GoogleSecretsBuilder(),
        new OpenTelemetrySecretsBuilder(),
        new PrimitiveSecretBuilder(Secrets.CosmosDbConnectionString, "ConnectionStrings:cosmos-db", configuration)
    ];

    public string[] SecretsKeys => [.. Builders.Select(b => b.SecretKey)];

    public void Construct(IDictionary<string, string> secretKeys)
    {
        foreach (var builder in Builders)
        {
            if (secretKeys.TryGetValue(builder.SecretKey, out var secretValue) && !string.IsNullOrEmpty(secretValue))
            {
                var configValues = builder.WithConfiguration(secretValue);

                foreach (var config in configValues)
                {
                    configurationProvider.Set(config.Key, config.Value);
                }
            }
        }
    }
}
