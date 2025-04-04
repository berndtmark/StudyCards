using StudyCards.Application.Interfaces;
using StudyCards.Application.Interfaces.Repositories;
using StudyCards.Domain.Entities;

namespace StudyCards.Application.UseCases.CardStudy.Get;

public class GetCardsToStudyUseCaseRequest
{
    public Guid DeckId { get; set; }
}

public class GetCardsToStudyUseCase(ICardRepository cardRepository) : IUseCase<GetCardsToStudyUseCaseRequest, IEnumerable<Card>>
{
    public Task<IEnumerable<Card>> Handle(GetCardsToStudyUseCaseRequest request)
    {
        var cards = cardRepository.GetByDeck(request.DeckId);

        throw new NotImplementedException();
    }
}
