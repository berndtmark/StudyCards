using StudyCards.Domain.Entities;
using StudyCards.Domain.Enums;
using StudyCards.Domain.Interfaces;

namespace StudyCards.Domain.State.AnkiState.States;

public class ReviewingState : ICardReviewState
{
    public (CardReviewStatus, ReviewPhase) Schedule(CardReviewStatus cardStatus, CardReview[] pastCardReviews, CardDifficulty difficulty, int repeatCount, AnkiScheduleConfiguration configuration)
    {
        // Go to the relearning phase
        if (IsCardForgotten(pastCardReviews, difficulty, repeatCount, configuration))
        {
            var (updatedCard, _) = new RelearningState().Schedule(cardStatus, pastCardReviews, difficulty, repeatCount, configuration);
            return (updatedCard, ReviewPhase.Relearning);
        }

        var easeChange = difficulty switch
        {
            CardDifficulty.Easy => configuration.EASE_BONUS,
            CardDifficulty.Medium => 0,
            CardDifficulty.Hard => -configuration.EASE_BONUS,
            _ => 0
        };

        var newEase = Math.Max(configuration.MIN_EASE, cardStatus.EaseFactor + easeChange);

        double multiplier = difficulty switch
        {
            CardDifficulty.Hard => configuration.HARD_REVIEW_MULTIPLIER,
            CardDifficulty.Easy => newEase * configuration.EASY_REVIEW_MULTIPLIER,
            CardDifficulty.Medium => newEase,
            _ => newEase
        };

        var newInterval = Math.Clamp(cardStatus.IntervalInDays * multiplier, configuration.MIN_INTERVAL, configuration.MAX_INTERVAL);

        return (
            cardStatus with
            {
                EaseFactor = newEase,
                IntervalInDays = (int)Math.Round(newInterval),
                NextReviewDate = DateTime.UtcNow.AddDays(newInterval),
            },
            ReviewPhase.Reviewing
        );
    }

    private static bool IsCardForgotten(CardReview[] pastCardReviews, CardDifficulty currentDifficulty, int repeatCount, AnkiScheduleConfiguration config)
    {
        var lastReview = pastCardReviews?.LastOrDefault();

        // Condition 1: Two consecutive Hard ratings
        bool sustainedHard = currentDifficulty == CardDifficulty.Hard &&
                             lastReview?.CardDifficulty == CardDifficulty.Hard;

        // Condition 2: Card was repeated AND current rating is Hard (not Medium)
        bool repeatAndStruggled = repeatCount > 0 && currentDifficulty == CardDifficulty.Hard;

        // Condition 3: Card was repeated too many times
        bool tooManyRepeats = repeatCount >= config.FORGET_REPEAT_THRESHOLD &&
                      currentDifficulty != CardDifficulty.Easy;

        return sustainedHard || repeatAndStruggled || tooManyRepeats;
    }
}
