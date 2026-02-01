using StudyCards.Domain.Entities;
using StudyCards.Domain.Interfaces;

namespace StudyCards.Domain.Study.Strategy.CardsToStudyStrategy.Strategies;

public class RandomCardStrategy : ICardsToStudyStrategy
{
    public IEnumerable<Card> GetCards(IEnumerable<Card> cards, int noCardsToSelect, int noNewCardsToSelect)
    {
        Random random = new();

        // 1. Select new cards first
        var newCards = cards
            .Where(x => x.CardReviewStatus.ReviewCount == 0)
            .OrderBy(x => random.Next())
            .Take(noNewCardsToSelect)
            .ToList();

        // 2. Fill remaining slots with random cards
        var reviewCards = cards
            .Where(x => x.CardReviewStatus.ReviewCount > 0)
            .OrderBy(x => random.Next())
            .Take(noCardsToSelect - newCards.Count);

        return [.. newCards, .. reviewCards];
    }
}
