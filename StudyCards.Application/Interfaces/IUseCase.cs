namespace StudyCards.Application.Interfaces;

public interface IUseCase<TRequest, TResponse>
{
    public Task<TResponse> Handle();
}
