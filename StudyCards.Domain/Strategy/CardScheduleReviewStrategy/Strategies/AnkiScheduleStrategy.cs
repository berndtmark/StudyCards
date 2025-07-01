using StudyCards.Domain.Entities;
using StudyCards.Domain.Enums;
using StudyCards.Domain.Interfaces;

namespace StudyCards.Domain.Strategy.CardScheduleReviewStrategy.Strategies;

public class AnkiScheduleStrategy : ICardScheduleStrategy
{
    private const double MIN_INTERVAL = 1.0; // Minimum interval in days
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
            newInterval = 3.0;
        }
        else
        {
            // Use the current interval and ease factor to determine next interval
            newInterval = currentInterval * newEase;

            // Adjust interval based on difficulty
            newInterval *= difficulty switch
            {
                CardDifficulty.Easy => 1.3,
                CardDifficulty.Medium => 1.0,
                CardDifficulty.Hard => 0.7,
                _ => 1.0
            };
        }

        // Apply repeat penalty to interval
        if (repeatCount > 0)
        {
            var repeatMultiplier = Math.Max(0.5, 1 - 0.1 * repeatsToConsider);
            newInterval *= repeatMultiplier;
        }

        // Ensure minimum interval
        newInterval = Math.Max(MIN_INTERVAL, newInterval);

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
