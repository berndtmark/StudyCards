using StudyCards.Application.Interfaces;
using StudyCards.Application.Interfaces.Repositories;
using StudyCards.Domain.Entities;

namespace StudyCards.Application.UseCases.CardManagement.GetCards;

public class GetCardsRequest
{
    public Guid DeckId { get; set; }
}

public class GetCardsUseCase(ICardRepository cardRepository) : IUseCase<GetCardsRequest, IEnumerable<Card>>
{
    public async Task<IEnumerable<Card>> Handle(GetCardsRequest request)
    {
        return await cardRepository.GetByDeck(request.DeckId);
    }
}
