using Microsoft.Extensions.DependencyInjection;
using StudyCards.Application.Factory;
using StudyCards.Application.Interfaces;
using StudyCards.Application.Interfaces.CQRS;
using StudyCards.Application.Services;
using StudyCards.Domain.Interfaces;
using StudyCards.Domain.Strategy.CardScheduleReviewStrategy;
using StudyCards.Domain.Strategy.CardsToStudyStrategy;

namespace StudyCards.Application;

public static class ServicesConfiguration
{
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<ICardSelectionStudyFactory, CardSelectionStudyFactory>();
        services.AddTransient<ICardsToStudyStrategyContext, CardsToStudyStrategyContext>();
        services.AddTransient<ICardScheduleStrategyContext, CardScheduleStrategyContext>();
        services.AddTransient<IDeckCardCountService, DeckCardCountService>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(ICommand<>)));

        return services;
    }
}