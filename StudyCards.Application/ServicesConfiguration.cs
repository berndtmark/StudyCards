
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudyCards.Application.Configuration;
using StudyCards.Application.Factory;
using StudyCards.Application.Interfaces;
using StudyCards.Application.SecretsManager;
using StudyCards.Application.UseCases.CardManagement.AddCard;
using System.Reflection;

namespace StudyCards.Application;

public static class ServicesConfiguration
{
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SecretOptions>(options => configuration.GetSection(SecretOptions.Key).Bind(options));

        services.AddScoped<ISecretsManager, BitwardenSecretsManager>();
        services.AddTransient<IUseCaseFactory, UseCaseFactory>();
        services.AddUseCases();

        return services;
    }

    private static void AddUseCases(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var useCaseTypes = assembly.GetTypes()
            .Where(t => t.GetInterfaces().Any(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IUseCase<,>)))
            .ToList();

        foreach (var useCaseType in useCaseTypes)
        {
            var interfaceType = useCaseType.GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IUseCase<,>));
            services.AddTransient(interfaceType, useCaseType);
        }
    }
}
