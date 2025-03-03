using Microsoft.Extensions.DependencyInjection;
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

    public IUseCase<TInput, TOutput> Create<TInput, TOutput>()
    {
        return _serviceProvider.GetService<IUseCase<TInput, TOutput>>()
             ?? throw new InvalidOperationException($"No Use Case found for {typeof(TInput).Name} and {typeof(TOutput).Name}");
    }
}
