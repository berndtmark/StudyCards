using StudyCards.Application.Common;

namespace StudyCards.Application.Interfaces.CQRS;

public interface ICommandHandler<in TCommand, TResponse> where TCommand : ICommand<TResponse>
{
    Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken);
}
