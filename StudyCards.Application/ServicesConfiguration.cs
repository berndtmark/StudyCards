using Microsoft.Extensions.DependencyInjection;
using StudyCards.Application.Factory;
using StudyCards.Application.Interfaces;
using StudyCards.Application.Interfaces.CQRS;
using StudyCards.Application.Services;
using StudyCards.Domain.Interfaces;
using StudyCards.Domain.Strategy.CardStrategy;

namespace StudyCards.Application;

public static class ServicesConfiguration
{
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<ICardSelectionStudyFactory, CardSelectionStudyFactory>();
        services.AddTransient<ICardStrategyContext, CardStrategyContext>();
        services.AddTransient<IDeckCardCountService, DeckCardCountService>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(ICommand<>)));

        return services;
    }
}