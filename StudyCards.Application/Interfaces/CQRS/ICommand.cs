using MediatR;

namespace StudyCards.Application.Interfaces.CQRS;

public interface ICommand<TResponse> : IRequest<TResponse>
{
}
