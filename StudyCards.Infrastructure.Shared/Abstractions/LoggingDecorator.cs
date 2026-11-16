using Microsoft.Extensions.Logging;
using StudyCards.Application.Common;
using StudyCards.Application.Interfaces.CQRS;
using System.Diagnostics;

namespace StudyCards.Infrastructure.Shared.Abstractions;

internal static class LoggingDecorator
{
    internal sealed class QueryHandler<TQuery, TResponse>(IQueryHandler<TQuery, TResponse> innerHandler, ILogger<QueryHandler<TQuery, TResponse>> logger) :
        IQueryHandler<TQuery, TResponse> where TQuery : IQuery<TResponse>
    {
        public async Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken)
        {
            string queryName = typeof(TQuery).Name;
            logger.LogInformation("Processing query {Query}", queryName);

            Stopwatch stopwatch = Stopwatch.StartNew();
            Result<TResponse> result = await innerHandler.Handle(query, cancellationToken);
            stopwatch.Stop();

            if (result.IsSuccess)
            {
                logger.LogInformation("Completed query {Query} in {Time}ms", queryName, stopwatch.ElapsedMilliseconds);
            }
            else
            {
                logger.LogInformation("Completed query {Query} with validation/error {Error} in {Time}ms", queryName, result.ErrorMessage, stopwatch.ElapsedMilliseconds);
            }

            return result;
        }
    }

    internal sealed class CommandHandler<TCommand, TResponse>(ICommandHandler<TCommand, TResponse> innerHandler, ILogger<CommandHandler<TCommand, TResponse>> logger) :
        ICommandHandler<TCommand, TResponse> where TCommand : ICommand<TResponse>
    {
        public async Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken)
        {
            string commandName = typeof(TCommand).Name;
            logger.LogInformation("Processing command {Command}", commandName);

            Stopwatch stopwatch = Stopwatch.StartNew();
            Result<TResponse> result = await innerHandler.Handle(command, cancellationToken);
            stopwatch.Stop();

            if (result.IsSuccess)
            {
                logger.LogInformation("Completed command {Command} in {Time}ms", commandName, stopwatch.ElapsedMilliseconds);
            }
            else
            {
                logger.LogInformation("Completed command {Command} with validation/error {Error} in {Time}ms", commandName, result.ErrorMessage, stopwatch.ElapsedMilliseconds);
            }

            return result;
        }
    }
}