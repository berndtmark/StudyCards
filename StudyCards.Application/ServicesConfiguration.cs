
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudyCards.Application.Configuration;
using StudyCards.Application.Factory;
using StudyCards.Application.Interfaces;
using StudyCards.Application.SecretsManager;
using StudyCards.Application.UseCases.CardManagement.AddCard;

namespace StudyCards.Application;

public static class ServicesConfiguration
{
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SecretOptions>(options => configuration.GetSection(SecretOptions.Key).Bind(options));

        services.AddScoped<ISecretsManager, BitwardenSecretsManager>();
        services.AddTransient<IUseCaseFactory, UseCaseFactory>();
        services.AddTransient<IUseCase<AddCardRequest, string>, AddCardUseCase>();

        return services;
    }
}
