using Microsoft.Extensions.DependencyInjection;
using StudyCards.Application.Common;
using StudyCards.Application.Interfaces.CQRS;
using System.Collections.Concurrent;

namespace StudyCards.Infrastructure.Shared.Messaging;

internal class CQRSDispatcher(IServiceProvider serviceProvider) : ICQRSDispatcher
{
    private static readonly ConcurrentDictionary<Type, object> _commandHandlers = new();
    private static readonly ConcurrentDictionary<Type, object> _queryHandlers = new();

    public Task<Result<TResponse>> Send<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default)
    {
        var commandType = command.GetType();

        // Get or create the wrapper for this specific command type
        var wrapper = (CommandHandlerWrapper<TResponse>)_commandHandlers.GetOrAdd(commandType, t =>
        {
            // Create the specific wrapper type: CommandHandlerWrapperImpl<MyQuery, MyResponse>
            var wrapperType = typeof(CommandHandlerWrapperImpl<,>)
                .MakeGenericType(t, typeof(TResponse));

            // Instantiate the wrapper once and cache it
            return Activator.CreateInstance(wrapperType)
                   ?? throw new InvalidOperationException($"Could not create wrapper for {t}");
        });

        // Invoke
        return wrapper.Handle(command, serviceProvider, cancellationToken);
    }

    public Task<Result<TResponse>> Send<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default)
    {
        var queryType = query.GetType();

        // Get or create the wrapper for this specific query type
        var wrapper = (QueryHandlerWrapper<TResponse>)_queryHandlers.GetOrAdd(queryType, t =>
        {
            // Create the specific wrapper type: QueryHandlerWrapperImpl<MyQuery, MyResponse>
            var wrapperType = typeof(QueryHandlerWrapperImpl<,>)
                .MakeGenericType(t, typeof(TResponse));

            // Instantiate the wrapper once and cache it
            return Activator.CreateInstance(wrapperType)
                   ?? throw new InvalidOperationException($"Could not create wrapper for {t}");
        });

        // Invoke
        return wrapper.Handle(query, serviceProvider, cancellationToken);
    }
}

#region QueryHandlerWrapper
internal abstract class QueryHandlerWrapper<TResponse>
{
    public abstract Task<Result<TResponse>> Handle(
        IQuery<TResponse> query,
        IServiceProvider serviceProvider,
        CancellationToken cancellationToken);
}

internal class QueryHandlerWrapperImpl<TQuery, TResponse> : QueryHandlerWrapper<TResponse>
    where TQuery : IQuery<TResponse>
{
    public override Task<Result<TResponse>> Handle(
        IQuery<TResponse> query,
        IServiceProvider serviceProvider,
        CancellationToken cancellationToken)
    {
        var handler = serviceProvider.GetRequiredService<IQueryHandler<TQuery, TResponse>>();
        return handler.Handle((TQuery)query, cancellationToken);
    }
}
#endregion

#region CommandHandlerWrapper
internal abstract class CommandHandlerWrapper<TResponse>
{
    public abstract Task<Result<TResponse>> Handle(
        ICommand<TResponse> command,
        IServiceProvider serviceProvider,
        CancellationToken cancellationToken);
}

internal class CommandHandlerWrapperImpl<TCommand, TResponse> : CommandHandlerWrapper<TResponse>
    where TCommand : ICommand<TResponse>
{
    public override Task<Result<TResponse>> Handle(
        ICommand<TResponse> command,
        IServiceProvider serviceProvider,
        CancellationToken cancellationToken)
    {
        var handler = serviceProvider.GetRequiredService<ICommandHandler<TCommand, TResponse>>();
        return handler.Handle((TCommand)command, cancellationToken);
    }
}
#endregion
