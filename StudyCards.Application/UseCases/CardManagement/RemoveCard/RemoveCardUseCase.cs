using Microsoft.Extensions.Logging;
using StudyCards.Application.Interfaces;
using StudyCards.Application.Interfaces.UnitOfWork;

namespace StudyCards.Application.UseCases.CardManagement.RemoveCard;

public class RemoveCardUseCaseRequest
{
    public Guid CardId { get; set; }
    public Guid DeckId { get; set; }
}

public class RemoveCardUseCase(IUnitOfWork unitOfWork, ILogger<RemoveCardUseCase> logger) : IUseCase<RemoveCardUseCaseRequest, bool>
{
    public async Task<bool> Handle(RemoveCardUseCaseRequest request)
    {
        try
        {
            await unitOfWork.CardRepository.Remove(request.CardId, request.DeckId);
            await unitOfWork.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            logger.LogError("Failed to remove card {CardId}", request.CardId);
            return false;
        }
    }
}
