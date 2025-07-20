using StudyCards.Domain.Entities;
using StudyCards.Domain.Enums;
using StudyCards.Domain.Interfaces;

namespace StudyCards.Domain.State.AnkiState.States;

public class LearningState : ICardReviewState
{
    public (CardReviewStatus, ReviewPhase) Schedule(CardReviewStatus cardStatus, CardReview[] pastCardReviews, CardDifficulty difficulty, int repeatCount, AnkiScheduleConfiguration configuration)
    {
        var step = cardStatus.LearningStep ?? 0;

        // go back to the start of the learning phase
        if (difficulty == CardDifficulty.Hard || repeatCount > 0)
        {
            return (
                cardStatus with
                {
                    LearningStep = 0,
                    IntervalInDays = (int)Math.Round(configuration.LEARNING_STEPS[0]),
                    NextReviewDate = DateTime.UtcNow.AddDays(configuration.LEARNING_STEPS[0]),
                },
                ReviewPhase.Learning
            );
        }

        // If we've completed all steps, graduate
        var nextStep = step + 1;
        if (nextStep >= configuration.LEARNING_STEPS.Length)
        {
            double interval = difficulty == CardDifficulty.Easy ? configuration.GRADUATED_EASY_INTERVAL : configuration.MIN_INTERVAL;
            return (
                cardStatus with
                {
                    LearningStep = null,
                    IntervalInDays = (int)Math.Round(interval),
                    NextReviewDate = DateTime.UtcNow.AddDays(interval),
                    EaseFactor = configuration.INITIAL_EASE,
                },
                ReviewPhase.Reviewing
            );
        }

        // move to the next learning step
        var nextInterval = configuration.LEARNING_STEPS[nextStep];
        return (
            cardStatus with
            {
                LearningStep = nextStep,
                IntervalInDays = (int)Math.Round(nextInterval),
                NextReviewDate = DateTime.UtcNow.AddDays(nextInterval),
            },
            ReviewPhase.Learning
        );
    }
}