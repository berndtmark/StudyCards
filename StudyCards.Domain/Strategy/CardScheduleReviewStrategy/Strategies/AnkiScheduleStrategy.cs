using StudyCards.Domain.Entities;
using StudyCards.Domain.Enums;
using StudyCards.Domain.Interfaces;

namespace StudyCards.Domain.Strategy.CardScheduleReviewStrategy.Strategies;

public class AnkiScheduleStrategy : ICardScheduleStrategy
{
    private const double MIN_INTERVAL = 1.0; // Minimum interval in days
    private const double MAX_INTERVAL = 365.0; // Maximum interval in days
    private const double EASE_BONUS = 0.15; // How much to increase/decrease ease
    private const double MIN_EASE = 1.3; // Minimum ease factor
    private const double REPEAT_PENALTY = 0.2; // Penalty for each repeat
    private const int MAX_REPEATS_TO_CONSIDER = 3; // Maximum repeats to factor in

    public Card ScheduleNext(Card card, CardDifficulty difficulty, int repeatCount = 0)
    {
        var currentEase = card.CardReviewStatus.EaseFactor;
        var currentInterval = card.CardReviewStatus.IntervalInDays;

        // Calculate new ease factor
        var easeChange = difficulty switch
        {
            CardDifficulty.Easy => EASE_BONUS,
            CardDifficulty.Medium => 0,
            CardDifficulty.Hard => -EASE_BONUS,
            _ => 0
        };

        // Apply repeat penalty
        var repeatsToConsider = Math.Min(repeatCount, MAX_REPEATS_TO_CONSIDER);
        easeChange -= REPEAT_PENALTY * repeatsToConsider;

        // Calculate new ease, ensuring it doesn't go below minimum
        var newEase = Math.Max(MIN_EASE, currentEase + easeChange);

        // Calculate new interval
        double newInterval;
        if (card.CardReviewStatus.ReviewCount == 0)
        {
            newInterval = MIN_INTERVAL;
        }
        else if (card.CardReviewStatus.ReviewCount == 1)
        {
            newInterval = 2.0;
        }
        else
        {
            // Use the current interval and ease factor to determine next interval
            newInterval = currentInterval * newEase;
        }

        // Ensure min and max interval
        newInterval = Math.Clamp(newInterval, MIN_INTERVAL, MAX_INTERVAL);

        return card with
        {
            CardReviewStatus = card.CardReviewStatus with
            {
                EaseFactor = newEase,
                IntervalInDays = (int)Math.Round(newInterval),
                NextReviewDate = DateTime.UtcNow.AddDays(newInterval),
                ReviewCount = card.CardReviewStatus.ReviewCount + 1
            }
        };
    }
}
