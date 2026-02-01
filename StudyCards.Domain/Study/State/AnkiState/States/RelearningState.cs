using StudyCards.Domain.Entities;
using StudyCards.Domain.Enums;
using StudyCards.Domain.Interfaces;
using StudyCards.Domain.Study.State.AnkiState;

namespace StudyCards.Domain.Study.State.AnkiState.States;

public class RelearningState : ICardReviewState
{
    public (CardReviewStatus, ReviewPhase) Schedule(CardReviewStatus cardStatus, CardReview[] pastCardReviews, CardDifficulty difficulty, int repeatCount, AnkiScheduleConfiguration configuration)
    {
        var step = cardStatus.LearningStep ?? 0;

        // Handle repeated failure at step 0
        if (step == 0)
        {
            if (difficulty == CardDifficulty.Hard || repeatCount > 0)
            {
                var newEase = Math.Max(
                    configuration.MIN_EASE,
                    cardStatus.EaseFactor - configuration.LAPSE_EASE_PENALTY
                      - Math.Max(0, Math.Min(repeatCount - 1, configuration.MAX_REPEATS_TO_CONSIDER) * configuration.LAPSE_EASE_PENALTY)
                );

                return (
                    cardStatus with
                    {
                        EaseFactor = newEase,
                        LearningStep = 0,
                        IntervalInDays = (int)Math.Round(configuration.RELEARNING_STEPS[0]),
                        NextReviewDate = DateTime.UtcNow.AddDays(configuration.RELEARNING_STEPS[0]),
                    },
                    ReviewPhase.Relearning
                );
            }

            // Successful recall — move to next step
            var nextStep = 1;
            var interval = configuration.RELEARNING_STEPS[nextStep];
            return (
                cardStatus with
                {
                    LearningStep = nextStep,
                    IntervalInDays = (int)Math.Round(interval),
                    NextReviewDate = DateTime.UtcNow.AddDays(interval),
                },
                ReviewPhase.Relearning
            );
        }

        // If we've completed all steps, graduate
        var nextStepProgress = step + 1;
        if (nextStepProgress >= configuration.RELEARNING_STEPS.Length)
        {
            var graduatedInterval = configuration.RELEARNING_GRADUATED_INTERVAL;
            return (
                cardStatus with
                {
                    LearningStep = null,
                    IntervalInDays = (int)Math.Round(graduatedInterval),
                    NextReviewDate = DateTime.UtcNow.AddDays(graduatedInterval),
                },
                ReviewPhase.Reviewing
            );
        }

        // Move to next relearning step
        var nextInterval = configuration.RELEARNING_STEPS[nextStepProgress];
        return (
            cardStatus with
            {
                LearningStep = nextStepProgress,
                IntervalInDays = (int)Math.Round(nextInterval),
                NextReviewDate = DateTime.UtcNow.AddDays(nextInterval),
            },
            ReviewPhase.Relearning
        );
    }
}
