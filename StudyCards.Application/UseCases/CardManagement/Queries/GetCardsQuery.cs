using StudyCards.Application.Interfaces.CQRS;
using StudyCards.Application.Interfaces.Repositories;
using StudyCards.Domain.Entities;

namespace StudyCards.Application.UseCases.CardManagement.Queries;

public class GetCardsQuery : IQuery<IEnumerable<Card>>
{
    public Guid DeckId { get; set; }
}

public class GetCardsQueryHandler(ICardRepository cardRepository) : IQueryHandler<GetCardsQuery, IEnumerable<Card>>
{
    public async Task<IEnumerable<Card>> Handle(GetCardsQuery request, CancellationToken cancellationToken)
    {
        return await cardRepository.GetByDeck(request.DeckId, cancellationToken);
    }
}
