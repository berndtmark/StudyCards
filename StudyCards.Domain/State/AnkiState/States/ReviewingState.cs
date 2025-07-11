using StudyCards.Domain.Entities;
using StudyCards.Domain.Enums;
using StudyCards.Domain.Interfaces;

namespace StudyCards.Domain.State.AnkiState.States;

public class ReviewingState : ICardReviewState
{
    public (CardReviewStatus, ReviewPhase) Schedule(CardReviewStatus cardStatus, CardDifficulty difficulty, int repeatCount, AnkiScheduleConfiguration configuration)
    {
        // Go to the relearning phase
        if (difficulty == CardDifficulty.Hard || repeatCount > 0)
        {
            var (updatedCard, _) = new RelearningState().Schedule(cardStatus, difficulty, repeatCount, configuration);
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
}
