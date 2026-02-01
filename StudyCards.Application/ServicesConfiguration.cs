using Microsoft.Extensions.DependencyInjection;
using StudyCards.Application.EventHandlers.Dispatcher;
using StudyCards.Application.Factory;
using StudyCards.Application.Interfaces;
using StudyCards.Application.Interfaces.CQRS;
using StudyCards.Application.Services;
using StudyCards.Domain.Interfaces;
using StudyCards.Domain.Interfaces.DomainEvent;
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
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(ICommand<>)));

        // Domain Events
        services.AddTransient<IDomainEventsDispatcher, DomainEventsDispatcher>();
        services.Scan(scan => scan.FromAssembliesOf(typeof(ServicesConfiguration))
            .AddClasses(classes => classes.AssignableTo(typeof(IDomainEventHandler<>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }
}