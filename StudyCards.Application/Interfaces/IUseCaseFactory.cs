namespace StudyCards.Application.Interfaces;

public interface IUseCaseFactory
{
    IUseCase<TRequest, TResponse> Create<TRequest, TResponse>();
}