using StudyCards.Application.Configuration;
using StudyCards.Application.Configuration.Options;
using StudyCards.Application.Interfaces;
using StudyCards.Infrastructure.Secrets.Interfaces;
using StudyCards.Infrastructure.Secrets.SecretsManager;

namespace StudyCards.Api.Configuration.SecretsConfiguration;

public static class SecretsConfiguration
{
    public static void AddSecretsConfiguration(this IHostApplicationBuilder builder)
    {
        if (OpenApiGen.IsOpenApiGeneration())
            return;

        // Create secrets manager directly
        var secretOptions = builder.Configuration.GetSection(SecretOptions.Key).Get<SecretOptions>()!;
        var secretClient = new SecretsClient(Microsoft.Extensions.Options.Options.Create(secretOptions));
        var secretsManager = new SecretsManager(secretClient);

        builder.Configuration.Add(new SecretConfigurationSource(secretsManager, builder.Configuration));

        builder.Services.AddSingleton<ISecretClient>(secretClient);
        builder.Services.AddSingleton<ISecretsManager, CachedSecretsManager>();
    }
}

public class SecretConfigurationSource(ISecretsManager secretsManager, IConfiguration configuration) : IConfigurationSource
{
    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new SecretConfigurationProvider(secretsManager, configuration);
    }
}

public class SecretConfigurationProvider(ISecretsManager secretsManager, IConfiguration configuration) : ConfigurationProvider
{
    private readonly ISecretsManager _secretsManager = secretsManager;

    public override void Load()
    {
        var secretsConstructor = new SecretsConstructor(configuration, this);
        var secretKeys = _secretsManager.GetSecrets(secretsConstructor.SecretsKeys);
        secretsConstructor.Construct(secretKeys);
    }
}
