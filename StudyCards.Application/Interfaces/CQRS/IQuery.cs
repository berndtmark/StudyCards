using MediatR;

namespace StudyCards.Application.Interfaces.CQRS;

public interface IQuery<TResponse> : IRequest<TResponse>
{
}
