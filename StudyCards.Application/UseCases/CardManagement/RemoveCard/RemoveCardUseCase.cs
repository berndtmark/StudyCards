using Microsoft.Extensions.Logging;
using StudyCards.Application.Interfaces;
using StudyCards.Application.Interfaces.Repositories;

namespace StudyCards.Application.UseCases.CardManagement.RemoveCard;

public class RemoveCardUseCaseRequest
{
    public Guid CardId { get; set; }
}

public class RemoveCardUseCase(ICardRepository cardRepository, ILogger<RemoveCardUseCase> logger) : IUseCase<RemoveCardUseCaseRequest, bool>
{
    public async Task<bool> Handle(RemoveCardUseCaseRequest request)
    {
        try
        {
            await cardRepository.Remove(request.CardId);
            return true;
        }
        catch (Exception)
        {
            logger.LogError("Failed to remove card {CardId}", request.CardId);
            return false;
        }
    }
}
