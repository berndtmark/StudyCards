
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudyCards.Application.Configuration;
using StudyCards.Application.Interfaces;
using StudyCards.Application.SecretsManager;

namespace StudyCards.Application;

public static class ApplicationConfiguration
{
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SecretOptions>(options => configuration.GetSection(SecretOptions.Key).Bind(options));

        services.AddScoped<ISecretsManager, BitwardenSecretsManager>();

        return services;
    }
}
