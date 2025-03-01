
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudyCards.Application.Configuration;
using StudyCards.Application.Interfaces;
using StudyCards.Application.SecretsManager;
using StudyCards.Data;

namespace StudyCards.Application;

public static class ApplicationConfiguration
{
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SecretOptions>(options => configuration.GetSection(SecretOptions.Key).Bind(options));

        services.AddScoped<ISecretsManager, BitwardenSecretsManager>();

        var serviceProvider = services.BuildServiceProvider();
        var secretsManager = serviceProvider.GetRequiredService<ISecretsManager>();
        var dbConnectionString = secretsManager.GetSecret(Secrets.CosmosDbConnectionString);
        services.ConfigureDataServices(dbConnectionString);

        return services;
    }
}
