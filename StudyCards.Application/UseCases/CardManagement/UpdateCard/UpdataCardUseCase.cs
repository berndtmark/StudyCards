using StudyCards.Application.Interfaces;
using StudyCards.Application.Interfaces.Repositories;
using StudyCards.Domain.Entities;

namespace StudyCards.Application.UseCases.CardManagement.UpdateCard;

public class UpdateCardUseCaseRequest
{
    public Guid CardId { get; set; }
    public string CardFront { get; set; } = string.Empty;
    public string CardBack { get; set; } = string.Empty;
}

public class UpdataCardUseCase(ICardRepository cardRepository) : IUseCase<UpdateCardUseCaseRequest, Card>
{
    public async Task<Card> Handle(UpdateCardUseCaseRequest request)
    {
        var currentCard = await cardRepository.Get(request.CardId);

        if (currentCard == null)
        {
            throw new Exception("Card not found");
        }

        var newCard = currentCard with
        {
            CardFront = request.CardFront,
            CardBack = request.CardBack
        };

        await cardRepository.Update(newCard);
        return newCard;
    }
}