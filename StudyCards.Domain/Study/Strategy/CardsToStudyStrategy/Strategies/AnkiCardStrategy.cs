using StudyCards.Domain.Entities;
using StudyCards.Domain.Interfaces;

namespace StudyCards.Domain.Study.Strategy.CardsToStudyStrategy.Strategies;

public class AnkiCardStrategy : ICardsToStudyStrategy
{
    public IEnumerable<Card> GetCards(IEnumerable<Card> cards, int noCardsToSelect, int noNewCardsToSelect)
    {
        var now = DateTime.UtcNow;
        var cardsList = cards.ToList();

        // 1. Select new cards (ReviewCount == 0)
        var newCards = cardsList
            .Where(card => card.CardReviewStatus.ReviewCount == 0)
            .OrderBy(card => card.CreatedDate)
            .Take(noNewCardsToSelect)
            .ToList();

        // 2. Select due review cards (excluding new cards already selected)
        var reviewCards = cardsList
            .Where(card => card.CardReviewStatus.ReviewCount > 0 && card.CardReviewStatus.NextReviewDate <= now && !newCards.Contains(card))
            .OrderBy(card => card.CardReviewStatus.NextReviewDate)
            .Take(noCardsToSelect - newCards.Count)
            .ToList();

        // 3. If not enough, fill with upcoming cards (not already selected)
        var selectedIds = new HashSet<Guid>(
            [.. newCards.Select(c => c.Id), .. reviewCards.Select(c => c.Id)]
        );

        var fillCount = noCardsToSelect - (newCards.Count + reviewCards.Count);
        var upcomingCards = new List<Card>();
        if (fillCount > 0)
        {
            upcomingCards = [.. cardsList
                .Where(card => !selectedIds.Contains(card.Id))
                .OrderBy(card => card.CardReviewStatus.NextReviewDate)
                .Take(fillCount)];
        }

        // 4. Combine and return
        return [.. newCards, .. reviewCards, .. upcomingCards];
    }
}
