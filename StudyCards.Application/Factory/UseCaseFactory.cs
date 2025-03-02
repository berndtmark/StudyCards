using StudyCards.Application.Interfaces;
using StudyCards.Application.UseCases.CardManagement.AddCard;

namespace StudyCards.Application.Factory;

public class UseCaseFactory : IUseCaseFactory
{
    private readonly IServiceProvider _serviceProvider;

    public UseCaseFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IUseCase<TRequest, TResponse> Create<TRequest, TResponse>(TRequest request)
    {
        var useCaseType = typeof(IUseCase<TRequest, TResponse>);
        var implementation = _serviceProvider.GetService(useCaseType)
            ?? throw new InvalidOperationException($"No implementation found for {useCaseType.Name}");

        return (IUseCase<TRequest, TResponse>)implementation;
    }
}
