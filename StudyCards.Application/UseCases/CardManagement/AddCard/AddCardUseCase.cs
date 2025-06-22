using Microsoft.Extensions.Logging;
using StudyCards.Application.Interfaces;
using StudyCards.Application.Interfaces.UnitOfWork;
using StudyCards.Domain.Entities;

namespace StudyCards.Application.UseCases.CardManagement.AddCard;

public class AddCardUseCaseRequest
{
    public Guid DeckId { get; set; }
    public string CardFront { get; set; } = string.Empty;
    public string CardBack { get; set; } = string.Empty;
}

public class AddCardUseCase(IUnitOfWork unitOfWork, ILogger<AddCardUseCase> logger, IDeckCardCountService deckCardCount) : IUseCase<AddCardUseCaseRequest, Card>
{
    public async Task<Card> Handle(AddCardUseCaseRequest request)
    {
        var card = new Card
        {
            Id = Guid.NewGuid(),
            DeckId = request.DeckId,
            CardFront = request.CardFront,
            CardBack = request.CardBack
        };

        try
        {
            var result = await unitOfWork.CardRepository.Add(card);
            await deckCardCount.UpdateDeckCardCount(request.DeckId, unitOfWork, 1);

            await unitOfWork.SaveChangesAsync();
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to add card to deck {DeckId}", request.DeckId);
            return default!;
        }
    }
}
