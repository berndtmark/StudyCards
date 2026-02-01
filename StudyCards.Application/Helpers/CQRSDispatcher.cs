using Microsoft.Extensions.DependencyInjection;
using StudyCards.Application.Common;
using StudyCards.Application.Interfaces.CQRS;
using System.Collections.Concurrent;

namespace StudyCards.Application.Helpers;

public class CQRSDispatcher(IServiceProvider serviceProvider) : ICQRSDispatcher
{
    private static readonly ConcurrentDictionary<Type, Type> _commandHandlers = new();
    private static readonly ConcurrentDictionary<Type, Type> _queryHandlers = new();

    public Task<Result<TResponse>> Send<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default)
    {
        var type = command.GetType();

        // Fast lookup
        var handlerType = _commandHandlers.GetOrAdd(type, t =>
            typeof(ICommandHandler<,>).MakeGenericType(t, typeof(TResponse)));

        dynamic handler = serviceProvider.GetRequiredService(handlerType);
        return handler.Handle((dynamic)command, cancellationToken);
    }

    public Task<Result<TResponse>> Send<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default)
    {
        var type = query.GetType();

        // Fast lookup
        var handlerType = _queryHandlers.GetOrAdd(type, t =>
            typeof(IQueryHandler<,>).MakeGenericType(t, typeof(TResponse)));

        dynamic handler = serviceProvider.GetRequiredService(handlerType);
        return handler.Handle((dynamic)query, cancellationToken);
    }
}