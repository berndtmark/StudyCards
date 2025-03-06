using StudyCards.Application.Configuration;
using StudyCards.Application.Interfaces;
using StudyCards.Application.SecretsManager;
using StudyCards.Server.Configuration.Options;
using System.Text.Json;

namespace StudyCards.Server.Configuration;

public static class SecretsConfiguration
{
    public static IServiceCollection AddSecretsConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        // Create secrets manager directly
        var secretOptions = configuration.GetSection(SecretOptions.Key).Get<SecretOptions>()!;
        var secretClient = new SecretsClient(Microsoft.Extensions.Options.Options.Create(secretOptions));
        var secretsManager = new SecretsManager(secretClient);

        // Load secrets
        var secrets = secretsManager.GetSecrets(Secrets.CosmosDbConnectionString, Secrets.GoogleAuthOptions);
        var googleAuthOptions = JsonSerializer.Deserialize<GoogleAuthOptions>(secrets[Secrets.GoogleAuthOptions])!;

        // Update configuration
        configuration.GetSection("ConnectionStrings")["CosmosDb"] = secrets[Secrets.CosmosDbConnectionString];
        configuration.GetSection("GoogleAuth:ClientId").Value = googleAuthOptions.ClientId;
        configuration.GetSection("GoogleAuth:ClientSecret").Value = googleAuthOptions.ClientSecret;

        // Register services after configuration is updated
        services.Configure<SecretOptions>(options => configuration.GetSection(SecretOptions.Key).Bind(options));
        services.AddSingleton<ISecretClient>(secretClient);
        services.AddSingleton<ISecretsManager, CachedSecretsManager>();

        services.AddSingleton(configuration);

        return services;
    }

}
