using Microsoft.Extensions.DependencyInjection;
using StudyCards.Application.Interfaces.CQRS;
using StudyCards.Domain.Interfaces.DomainEvent;
using StudyCards.Infrastructure.Shared.Abstractions;
using StudyCards.Infrastructure.Shared.Messaging;

namespace StudyCards.Infrastructure.Shared;

public static class ServicesConfiguration
{
    public static IServiceCollection ConfigureInfrastructureSharedServices(this IServiceCollection services)
    {
        // DOMAIN EVENTS
        services.AddTransient<IDomainEventsDispatcher, DomainEventsDispatcher>();
        services.Scan(scan => scan.FromAssembliesOf(typeof(Application.ServicesConfiguration))
            .AddClasses(classes => classes.AssignableTo(typeof(IDomainEventHandler<>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        // CQRS
        services.Scan(scan => scan.FromAssembliesOf(typeof(Application.ServicesConfiguration))
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime());
        services.AddScoped<ICQRSDispatcher, CQRSDispatcher>();
        services.Decorate(typeof(IQueryHandler<,>), typeof(LoggingDecorator.QueryHandler<,>));
        services.Decorate(typeof(ICommandHandler<,>), typeof(LoggingDecorator.CommandHandler<,>));

        return services;
    }
}
