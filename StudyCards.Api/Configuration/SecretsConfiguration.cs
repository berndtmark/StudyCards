using StudyCards.Api.Configuration.Options;
using StudyCards.Application.Configuration;
using StudyCards.Application.Configuration.Options;
using StudyCards.Application.Interfaces;
using StudyCards.Application.SecretsManager;
using System.Text.Json;

namespace StudyCards.Api.Configuration;

public static class SecretsConfiguration
{
    public static void AddSecretsConfiguration(this WebApplicationBuilder builder)
    {
        if (OpenApiGen.IsOpenApiGeneration())
            return;

        // Create secrets manager directly
        var secretOptions = builder.Configuration.GetSection(SecretOptions.Key).Get<SecretOptions>()!;
        var secretClient = new SecretsClient(Microsoft.Extensions.Options.Options.Create(secretOptions));
        var secretsManager = new SecretsManager(secretClient);

        ((IConfigurationBuilder)builder.Configuration).Add(new SecretConfigurationSource(secretsManager, builder.Configuration));

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
        var secrets = _secretsManager.GetSecrets(
            Secrets.CosmosDbConnectionString,
            Secrets.GoogleAuthOptions
        );

        Data["ConnectionStrings:CosmosDb"] = GetFirstNonNull(configuration.GetSection("ConnectionStrings")["CosmosDb"], secrets[Secrets.CosmosDbConnectionString]);

        var googleAuthOptions = JsonSerializer.Deserialize<GoogleAuthOptions>(
            secrets[Secrets.GoogleAuthOptions]
        )!;

        Data["GoogleAuth:ClientId"] = googleAuthOptions.ClientId;
        Data["GoogleAuth:ClientSecret"] = googleAuthOptions.ClientSecret;
    }

    private static string GetFirstNonNull(string? currentValue, string newValue) => string.IsNullOrEmpty(currentValue) ? newValue : currentValue;
}
