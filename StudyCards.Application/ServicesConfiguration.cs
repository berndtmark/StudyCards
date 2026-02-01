using Microsoft.Extensions.DependencyInjection;
using StudyCards.Application.Factory;
using StudyCards.Application.Interfaces;
using StudyCards.Application.Services;
using StudyCards.Domain.Interfaces;
using StudyCards.Domain.Study.Strategy.CardScheduleReviewStrategy;
using StudyCards.Domain.Study.Strategy.CardsToStudyStrategy;

namespace StudyCards.Application;

public static class ServicesConfiguration
{
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<ICardSelectionStudyFactory, CardSelectionStudyFactory>();
        services.AddTransient<ICardsToStudyStrategyContext, CardsToStudyStrategyContext>();
        services.AddTransient<ICardScheduleStrategyContext, CardScheduleStrategyContext>();
        services.AddTransient<IDeckCardCountService, DeckCardCountService>();

        return services;
    }
}