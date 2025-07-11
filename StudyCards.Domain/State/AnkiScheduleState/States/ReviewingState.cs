using StudyCards.Domain.Entities;
using StudyCards.Domain.Enums;

namespace StudyCards.Domain.State.AnkiScheduleState.States;

public interface ICardReviewState
{
    (CardReviewStatus UpdatedCardStatus, ReviewPhase NextPhase) Schedule(CardReviewStatus cardStatus, CardDifficulty difficulty, int repeatCount, AnkiScheduleConfiguration configuration);
}

public class LearningState : ICardReviewState
{
    public (CardReviewStatus, ReviewPhase) Schedule(CardReviewStatus cardStatus, CardDifficulty difficulty, int repeatCount, AnkiScheduleConfiguration configuration)
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

public class RelearningState : ICardReviewState
{
    public (CardReviewStatus, ReviewPhase) Schedule(CardReviewStatus cardStatus, CardDifficulty difficulty, int repeatCount, AnkiScheduleConfiguration configuration)
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
            const double graduatedInterval = 2.0;
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

public class AnkiReviewStateMachine
{
    public Card Schedule(Card card, CardDifficulty difficulty, int repeatCount)
    {
        var currentPhase = card.CardReviewStatus.CurrentPhase ?? InferPhase(card);
        var configuration = new AnkiScheduleConfiguration();

        ICardReviewState state = currentPhase switch
        {
            ReviewPhase.Learning => new LearningState(),
            ReviewPhase.Relearning => new RelearningState(),
            ReviewPhase.Reviewing => new ReviewingState(),
            _ => throw new InvalidOperationException("Unknown review phase")
        };

        var (updatedCardStatus, nextPhase) = state.Schedule(card.CardReviewStatus, difficulty, repeatCount, configuration);

        //if (nextPhase is not null && nextPhase != card.CardReviewStatus.CurrentPhase)
        //{
        //    card = card with
        //    {
        //        CardReviewStatus = updatedCardStatus with
        //        {
        //            CurrentPhase = nextPhase.Value,
        //        }
        //    };
        //}

        card = card with
        {
            CardReviewStatus = updatedCardStatus with
            {
                CurrentPhase = nextPhase,
                ReviewCount = updatedCardStatus.ReviewCount + 1,
            }
        };

        return card;
    }

    // todo check if old cards default to Learning - then remove
    // Used for existing cards that dont have phase set
    private static ReviewPhase InferPhase(Card card)
    {
        var count = card.CardReviewStatus.ReviewCount;

        if (count <= 2)
            return ReviewPhase.Learning;

        return ReviewPhase.Reviewing;
    }
}