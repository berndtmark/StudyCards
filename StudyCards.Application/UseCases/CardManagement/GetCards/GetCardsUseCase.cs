using StudyCards.Application.Interfaces;
using StudyCards.Application.Interfaces.Repositories;
using StudyCards.Domain.Entities;

namespace StudyCards.Application.UseCases.CardManagement.GetCards;

public class GetCardsUseCaseRequest
{
    public Guid DeckId { get; set; }
}

public class GetCardsUseCase(ICardRepository cardRepository) : IUseCase<GetCardsUseCaseRequest, IEnumerable<Card>>
{
    public async Task<IEnumerable<Card>> Handle(GetCardsUseCaseRequest request)
    {
        return await cardRepository.GetByDeck(request.DeckId);
    }
}
