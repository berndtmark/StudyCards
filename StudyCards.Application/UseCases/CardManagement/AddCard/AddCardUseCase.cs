using Microsoft.Extensions.Logging;
using StudyCards.Application.Interfaces;
using StudyCards.Application.Interfaces.Repositories;
using StudyCards.Domain.Entities;

namespace StudyCards.Application.UseCases.CardManagement.AddCard;

public class AddCardUseCaseRequest
{
    public Guid DeckId { get; set; }
    public string CardFront { get; set; } = string.Empty;
    public string CardBack { get; set; } = string.Empty;
}

public class AddCardUseCase(ICardRepository cardRepository, ILogger<AddCardUseCase> logger) : IUseCase<AddCardUseCaseRequest, bool>
{
    public async Task<bool> Handle(AddCardUseCaseRequest request)
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
            await cardRepository.Add(card);
            return true;
        }
        catch (Exception)
        {
            logger.LogError("Failed to add card to deck {DeckId}", request.DeckId);
            return false;
        }
    }
}
