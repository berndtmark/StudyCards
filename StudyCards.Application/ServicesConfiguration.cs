
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudyCards.Application.Factory;
using StudyCards.Application.Interfaces;
using StudyCards.Application.Services;
using StudyCards.Application.Strategy.CardStrategy;
using System.Reflection;

namespace StudyCards.Application;

public static class ServicesConfiguration
{
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IUseCaseFactory, UseCaseFactory>();
        services.AddTransient<ICardSelectionStudyFactory, CardSelectionStudyFactory>();
        services.AddTransient<ICardStrategyContext, CardStrategyContext>();
        services.AddTransient<IDeckCardCountService, DeckCardCountService>();
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
