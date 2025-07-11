using Microsoft.Extensions.Logging;
using StudyCards.Application.Exceptions;
using StudyCards.Application.Interfaces.CQRS;
using StudyCards.Application.Interfaces.UnitOfWork;
using StudyCards.Domain.Entities;
using StudyCards.Domain.Enums;
using StudyCards.Domain.Extensions;
using StudyCards.Domain.Interfaces;
using StudyCards.Domain.Strategy.CardScheduleReviewStrategy;
using StudyCards.Domain.Strategy.CardScheduleReviewStrategy.Strategies;

namespace StudyCards.Application.UseCases.CardStudy.Commands;

public class ReviewCardsCommand : ICommand<IList<Card>>
{
    public Guid DeckId { get; set; }
    public IList<CardReviewed> CardReviews { get; set; } = new List<CardReviewed>();

    public class CardReviewed
    {   
        public Guid CardId { get; set; }
        public CardDifficulty CardDifficulty { get; set; }
        public int? RepeatCount { get; set; }
    }
}

public class ReviewCardsCommandHandler(IUnitOfWork unitOfWork, ICardScheduleStrategyContext cardScheduleStrategy, ILogger<ReviewCardsCommandHandler> logger) : ICommandHandler<ReviewCardsCommand, IList<Card>>
{
    public async Task<IList<Card>> Handle(ReviewCardsCommand request, CancellationToken cancellationToken)
    {
        var response = new List<Card>();

        var reviewedCardIds = request.CardReviews.Select(cr => cr.CardId).ToArray();
        var reviewMap = request.CardReviews.ToDictionary(r => r.CardId);

        var cards = await unitOfWork.CardRepository.Get(reviewedCardIds, request.DeckId, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Deck), request.DeckId);

        // schedule cards next review
        var scheduledCards = ScheduleCards(cardScheduleStrategy, cards, reviewMap);

        foreach (var card in scheduledCards)
        {
            var review = reviewMap[card.Id];

            var cardReview = new CardReview
            {
                CardReviewId = Guid.NewGuid(),
                CardDifficulty = review.CardDifficulty,
                RepeatCount = review.RepeatCount ?? 0,
                ReviewDate = DateTime.UtcNow
            };

            var updatedCard = card.AddReview(cardReview);
            var result = unitOfWork.CardRepository.Update(updatedCard);
            response.Add(result);

            logger.LogInformation("Scheduling card {CardId}: difficulty={Difficulty}, interval={Interval}, nextPhase={NextPhase}", result.Id, review.CardDifficulty, result.CardReviewStatus.IntervalInDays, result.CardReviewStatus.CurrentPhase);
        }

        // update deck last review status
        await UpdateDeck(unitOfWork, request.DeckId, request.CardReviews.Count, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return response;
    }

    private async Task UpdateDeck(IUnitOfWork unitOfWork, Guid deckId, int cardReviewCount, CancellationToken cancellationToken)
    {
        var deck = await unitOfWork.DeckRepository.Get(deckId, cancellationToken);

        var isFirstReviewToday = !deck!.DeckReviewStatus.LastReview.Date.IsSameDay();
        var updatedDeck = deck with
        {
            DeckReviewStatus = new DeckReviewStatus
            {
                LastReview = DateTime.UtcNow,
                ReviewCount = isFirstReviewToday ? cardReviewCount : deck.DeckReviewStatus.ReviewCount + cardReviewCount,
            }
        };

        unitOfWork.DeckRepository.Update(updatedDeck);
    }

    private IEnumerable<Card> ScheduleCards(ICardScheduleStrategyContext cardScheduleStrategy, IEnumerable<Card> cards, IDictionary<Guid, ReviewCardsCommand.CardReviewed> reviewMap)
    {
        cardScheduleStrategy.AddCards(cards);
        cardScheduleStrategy.SetStrategy(new AnkiScheduleStrategy());

        var cardSchedules = cards.Select(c =>
        {
            var review = reviewMap[c.Id];
            return new CardSchedule
            {
                Card = c,
                Difficulty = review.CardDifficulty,
                RepeatCount = review.RepeatCount ?? 0
            };
        });

        return cardScheduleStrategy.ScheduleCards(cardSchedules);
    }
}
