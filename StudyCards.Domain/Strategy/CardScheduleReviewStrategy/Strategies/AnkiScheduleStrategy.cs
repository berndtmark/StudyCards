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
    private static readonly double[] LEARNING_STEPS = [1, 2]; // Learning steps for new cards

    public Card ScheduleNext(Card card, CardDifficulty difficulty, int repeatCount = 0)
    {
        var currentEase = card.CardReviewStatus.EaseFactor;
        var currentInterval = card.CardReviewStatus.IntervalInDays;
        var reviewCount = card.CardReviewStatus.ReviewCount;

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
        double newInterval = 0;
        //if (card.CardReviewStatus.ReviewCount == 0)
        //{
        //    newInterval = MIN_INTERVAL;
        //}
        //else if (card.CardReviewStatus.ReviewCount == 1)
        //{
        //    newInterval = 2.0;
        //}
        //else
        //{
        //    // Use the current interval and ease factor to determine next interval
        //    newInterval = currentInterval * newEase;
        //}

        if (reviewCount < LEARNING_STEPS.Length)
        {

        }
        else
        {
            newInterval = CalculateGraduatedInterval(card, difficulty, newEase);
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

    private double CalculateGraduatedInterval(Card card, CardDifficulty difficulty, double newEase)
    {
        var currentInterval = card.CardReviewStatus.IntervalInDays;

        if (difficulty == CardDifficulty.Hard)
        {
            HandleLapse(card);
        }

        return difficulty switch
        {
            CardDifficulty.Hard => currentInterval * 1.2, // Hard multiplier
            CardDifficulty.Medium => currentInterval * newEase,
            CardDifficulty.Easy => currentInterval * newEase * 1.3, // Easy bonus
            _ => currentInterval * newEase
        };
    }

    private Card HandleLearningPhase(Card card, CardDifficulty difficulty, int repeatCount)
    {
        var reviewCount = card.CardReviewStatus.ReviewCount;

        // Assume CardReviewStatus has a LearningStep property (you'd need to add this)
        var currentLearningStep = card.CardReviewStatus.LearningStep ?? 0;

        if (difficulty == CardDifficulty.Hard || repeatCount > 0)
        {
            // Reset to first learning step but keep ReviewCount intact
            return card with
            {
                CardReviewStatus = card.CardReviewStatus with
                {
                    LearningStep = 0, // Reset learning progress
                    IntervalInDays = (int)Math.Round(LEARNING_STEPS[0]),
                    NextReviewDate = DateTime.UtcNow.AddDays(LEARNING_STEPS[0]),
                    ReviewCount = reviewCount + 1 // Still increment for logging
                }
            };
        }

        // Progress to next learning step or graduate
        var nextLearningStep = currentLearningStep + 1;
        if (nextLearningStep >= LEARNING_STEPS.Length)
        {
            // Graduate to review phase
            var initialInterval = difficulty == CardDifficulty.Easy ? 4.0 : MIN_INTERVAL;
            return card with
            {
                CardReviewStatus = card.CardReviewStatus with
                {
                    LearningStep = null, // No longer in learning phase
                    IntervalInDays = (int)Math.Round(initialInterval),
                    NextReviewDate = DateTime.UtcNow.AddDays(initialInterval),
                    ReviewCount = reviewCount + 1,
                    EaseFactor = 2.5
                }
            };
        }

        return card with
        {
            CardReviewStatus = card.CardReviewStatus with
            {
                LearningStep = nextLearningStep,
                IntervalInDays = (int)Math.Round(LEARNING_STEPS[nextLearningStep]),
                NextReviewDate = DateTime.UtcNow.AddDays(LEARNING_STEPS[nextLearningStep]),
                ReviewCount = reviewCount + 1
            }
        };
    }

    private Card HandleLapse(Card card)
    {
        const double LAPSE_INTERVAL = 1.0;
        const double LAPSE_EASE_PENALTY = 0.2;

        var currentEase = card.CardReviewStatus.EaseFactor;
        var newEase = Math.Max(MIN_EASE, currentEase - LAPSE_EASE_PENALTY);

        return card with
        {
            CardReviewStatus = card.CardReviewStatus with
            {
                EaseFactor = newEase,
                IntervalInDays = (int)Math.Round(LAPSE_INTERVAL),
                NextReviewDate = DateTime.UtcNow.AddDays(LAPSE_INTERVAL),
                ReviewCount = card.CardReviewStatus.ReviewCount + 1,
                LearningStep = 0 // Optionally reintroduce learning steps
            }
        };
    }
}
