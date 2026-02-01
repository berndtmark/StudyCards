using StudyCards.Application.Common;

namespace StudyCards.Application.Interfaces.CQRS;

public interface ICQRSDispatcher
{
    Task<Result<TResponse>> Send<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default);
    Task<Result<TResponse>> Send<TResponse>(IQuery<TResponse> command, CancellationToken cancellationToken = default);
}
