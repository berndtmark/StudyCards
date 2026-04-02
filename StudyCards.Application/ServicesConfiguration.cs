using Microsoft.Extensions.DependencyInjection;
using StudyCards.Application.Interfaces.Services;
using StudyCards.Application.Services;
using StudyCards.Domain.Interfaces;
using StudyCards.Domain.Study.Strategy.CardScheduleReviewStrategy;
using StudyCards.Domain.Study.Strategy.CardsToStudyStrategy.Strategies;

namespace StudyCards.Application;

public static class ServicesConfiguration
{
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<ICardSelectionService, CardSelectionService>();
        services.AddTransient<ICardsToStudyStrategy, AnkiCardStrategy>();
        services.AddTransient<ICardsToStudyStrategy, RandomCardStrategy>();

        services.AddTransient<ICardScheduleStrategyContext, CardScheduleStrategyContext>();

        return services;
    }
}