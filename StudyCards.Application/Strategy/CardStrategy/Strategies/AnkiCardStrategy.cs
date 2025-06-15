using StudyCards.Application.Interfaces;
using StudyCards.Domain.Entities;
using StudyCards.Domain.Enums;

namespace StudyCards.Application.Strategy.CardStrategy.Strategies;

public class AnkiCardStrategy : ICardStrategy
{
    private const double MIN_INTERVAL = 1.0; // Minimum interval in days
    private const double DEFAULT_EASE = 2.5; // Default ease factor
    private const double EASE_BONUS = 0.15; // How much to increase/decrease ease
    private const double MIN_EASE = 1.3; // Minimum ease factor
    private const double REPEAT_PENALTY = 0.2; // Penalty for each repeat
    private const int MAX_REPEATS_TO_CONSIDER = 3; // Maximum repeats to factor in

    private double CalculateNextInterval(Card card)
    {
        if (!card.CardReviews.Any())
            return MIN_INTERVAL;

        var lastReview = card.CardReviews.OrderByDescending(r => r.ReviewDate).First();
        var daysSinceLastReview = (DateTime.UtcNow - lastReview.ReviewDate).TotalDays;
        
        // Calculate average ease based on last few reviews
        var recentReviews = card.CardReviews.OrderByDescending(r => r.ReviewDate).Take(5);
        var ease = DEFAULT_EASE;
        
        foreach (var review in recentReviews)
        {
            ease += review.CardDifficulty switch
            {
                CardDifficulty.Easy => EASE_BONUS,
                CardDifficulty.Medium => 0, // Medium difficulty doesn't change ease
                CardDifficulty.Hard => -EASE_BONUS,
                _ => 0
            };

            // Apply penalty for repeats
            if (review.RepeatCount.HasValue)
            {
                var repeatsToConsider = Math.Min(review.RepeatCount.Value, MAX_REPEATS_TO_CONSIDER);
                ease -= REPEAT_PENALTY * repeatsToConsider;
            }
        }

        ease = Math.Max(MIN_EASE, ease);

        // Calculate next interval based on performance
        var interval = daysSinceLastReview * ease;

        // Adjust interval based on recent performance
        if (recentReviews.All(r => r.CardDifficulty == CardDifficulty.Easy))
        {
            interval *= 1.5; // Significant increase for consistently easy cards
        }
        else if (recentReviews.All(r => r.CardDifficulty == CardDifficulty.Medium))
        {
            interval *= 1.2; // Modest increase for consistently medium cards
        }
        else if (recentReviews.Any(r => r.CardDifficulty == CardDifficulty.Hard))
        {
            interval *= 0.7; // Decrease for any hard reviews
        }

        // Further adjust interval based on total repeats in recent reviews
        var totalRepeats = recentReviews.Sum(r => r.RepeatCount ?? 0);
        if (totalRepeats > 0)
        {
            var repeatMultiplier = Math.Max(0.5, 1 - (0.1 * Math.Min(totalRepeats, MAX_REPEATS_TO_CONSIDER)));
            interval *= repeatMultiplier;
        }

        return Math.Max(MIN_INTERVAL, interval);
    }

    public IEnumerable<Card> GetCards(IEnumerable<Card> cards, int noCardsToSelect)
    {
        var now = DateTime.UtcNow;
        var cardsByDueDate = cards
            .Select(card => new
            {
                Card = card,
                NextReviewDue = now.AddDays(CalculateNextInterval(card))
            })
            .OrderBy(x => x.NextReviewDue)
            .Select(x => x.Card);

        // Return cards that are due for review
        return cardsByDueDate.Take(noCardsToSelect);
    }
}
