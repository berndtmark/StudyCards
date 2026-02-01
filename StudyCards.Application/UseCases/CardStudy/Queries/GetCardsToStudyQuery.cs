using StudyCards.Application.Common;
using StudyCards.Application.Exceptions;
using StudyCards.Application.Interfaces.CQRS;
using StudyCards.Application.Interfaces.Repositories;
using StudyCards.Domain.Entities;
using StudyCards.Domain.Enums;

namespace StudyCards.Application.UseCases.CardStudy.Queries;

public class GetCardsToStudyQuery : IQuery<IEnumerable<Card>>
{
    public Guid DeckId { get; set; }
    public CardStudyMethodology StudyMethodology { get; set; }
}

public class GetCardsToStudyQueryHandler(
    IDeckRepository deckRepository, 
    ICardRepository cardRepository) : IQueryHandler<GetCardsToStudyQuery, IEnumerable<Card>>
{
    public async Task<Result<IEnumerable<Card>>> Handle(GetCardsToStudyQuery request, CancellationToken cancellationToken)
    {
        var deck = await deckRepository.Get(request.DeckId, cancellationToken) ?? throw new EntityNotFoundException(nameof(Deck), request.DeckId);

        var (cardsToStudy, fillUnmet) = request.StudyMethodology switch
        {
            CardStudyMethodology.ContinuedReview => (Math.Min(deck.DeckSettings.ReviewsPerDay, deck.CardCount ?? deck.DeckSettings.ReviewsPerDay), false),
            CardStudyMethodology.Anki => (deck.CardNoToReview, true),
            _ => throw new NotImplementedException($"Study methodology {request.StudyMethodology} is not currently supported."),
        };

        var result = await cardRepository.GetCardsToStudy(request.DeckId, cardsToStudy, deck.DeckSettings.NewCardsPerDay, fillUnmet, cancellationToken);

        return Result<IEnumerable<Card>>.Success(result);
    }
}
