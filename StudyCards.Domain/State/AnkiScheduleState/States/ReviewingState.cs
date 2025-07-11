using StudyCards.Domain.Entities;
using StudyCards.Domain.Enums;

namespace StudyCards.Domain.State.AnkiScheduleState.States;

//public enum ReviewPhase
//{
//    Learning,
//    Reviewing,
//    Relearning
//}

public interface ICardReviewState
{
    (Card UpdatedCard, ReviewPhase? NextPhase) Schedule(Card card, CardDifficulty difficulty, int repeatCount);
}

public class LearningState : ICardReviewState
{
    private static readonly double[] LEARNING_STEPS = [1, 2];

    public (Card, ReviewPhase?) Schedule(Card card, CardDifficulty difficulty, int repeatCount)
    {
        var step = card.CardReviewStatus.LearningStep ?? 0;

        if (difficulty == CardDifficulty.Hard || repeatCount > 0)
        {
            return (
                card with
                {
                    CardReviewStatus = card.CardReviewStatus with
                    {
                        LearningStep = 0,
                        IntervalInDays = (int)Math.Round(LEARNING_STEPS[0]),
                        NextReviewDate = DateTime.UtcNow.AddDays(LEARNING_STEPS[0]),
                        ReviewCount = card.CardReviewStatus.ReviewCount + 1
                    }
                },
                null
            );
        }

        var nextStep = step + 1;
        if (nextStep >= LEARNING_STEPS.Length)
        {
            double interval = difficulty == CardDifficulty.Easy ? 4.0 : 1.0;
            return (
                card with
                {
                    CardReviewStatus = card.CardReviewStatus with
                    {
                        LearningStep = null,
                        IntervalInDays = (int)Math.Round(interval),
                        NextReviewDate = DateTime.UtcNow.AddDays(interval),
                        EaseFactor = 2.5,
                        ReviewCount = card.CardReviewStatus.ReviewCount + 1
                    }
                },
                ReviewPhase.Reviewing
            );
        }

        return (
            card with
            {
                CardReviewStatus = card.CardReviewStatus with
                {
                    LearningStep = nextStep,
                    IntervalInDays = (int)Math.Round(LEARNING_STEPS[nextStep]),
                    NextReviewDate = DateTime.UtcNow.AddDays(LEARNING_STEPS[nextStep]),
                    ReviewCount = card.CardReviewStatus.ReviewCount + 1
                }
            },
            null
        );
    }
}

public class ReviewingState : ICardReviewState
{
    private const double EASE_BONUS = 0.15;
    private const double MIN_EASE = 1.3;
    private const double MAX_INTERVAL = 365;
    private const double MIN_INTERVAL = 1;
    private const double REPEAT_PENALTY = 0.2;
    private const int MAX_REPEATS_TO_CONSIDER = 3;

    public (Card, ReviewPhase?) Schedule(Card card, CardDifficulty difficulty, int repeatCount)
    {
        if (difficulty == CardDifficulty.Hard || repeatCount > 0)
        {
            var (updatedCard, _) = new RelearningState().Schedule(card, difficulty, repeatCount);
            return (updatedCard, ReviewPhase.Relearning);
        }

        var easeChange = difficulty switch
        {
            CardDifficulty.Easy => EASE_BONUS,
            CardDifficulty.Medium => 0,
            CardDifficulty.Hard => -EASE_BONUS,
            _ => 0
        };

        var newEase = Math.Max(MIN_EASE, card.CardReviewStatus.EaseFactor + easeChange);

        double multiplier = difficulty switch
        {
            CardDifficulty.Hard => 1.2,
            CardDifficulty.Easy => newEase * 1.3,
            CardDifficulty.Medium => newEase,
            _ => newEase
        };

        var newInterval = Math.Clamp(card.CardReviewStatus.IntervalInDays * multiplier, MIN_INTERVAL, MAX_INTERVAL);

        return (
            card with
            {
                CardReviewStatus = card.CardReviewStatus with
                {
                    EaseFactor = newEase,
                    IntervalInDays = (int)Math.Round(newInterval),
                    NextReviewDate = DateTime.UtcNow.AddDays(newInterval),
                    ReviewCount = card.CardReviewStatus.ReviewCount + 1
                }
            },
            null
        );
    }
}

public class RelearningState : ICardReviewState
{
    private static readonly double[] RELEARNING_STEPS = [1, 3];
    private const double LAPSE_EASE_PENALTY = 0.2;
    private const double MIN_EASE = 1.3;

    public (Card, ReviewPhase?) Schedule(Card card, CardDifficulty difficulty, int repeatCount)
    {
        var step = card.CardReviewStatus.LearningStep ?? 0;

        // Handle repeated failure at step 0
        if (step == 0)
        {
            if (difficulty == CardDifficulty.Hard || repeatCount > 0)
            {
                var newEase = Math.Max(
                    MIN_EASE,
                    card.CardReviewStatus.EaseFactor - LAPSE_EASE_PENALTY
                      - Math.Max(0, Math.Min(repeatCount - 1, MAX_REPEATS_TO_CONSIDER) * LAPSE_EASE_PENALTY)
                );

                return (
                    card with
                    {
                        CardReviewStatus = card.CardReviewStatus with
                        {
                            EaseFactor = newEase,
                            LearningStep = 0,
                            IntervalInDays = (int)Math.Round(RELEARNING_STEPS[0]),
                            NextReviewDate = DateTime.UtcNow.AddDays(RELEARNING_STEPS[0]),
                            ReviewCount = card.CardReviewStatus.ReviewCount + 1
                        }
                    },
                    null
                );
            }

            // Successful recall — move to next step
            var nextStep = 1;
            var interval = RELEARNING_STEPS[nextStep];
            return (
                card with
                {
                    CardReviewStatus = card.CardReviewStatus with
                    {
                        LearningStep = nextStep,
                        IntervalInDays = (int)Math.Round(interval),
                        NextReviewDate = DateTime.UtcNow.AddDays(interval),
                        ReviewCount = card.CardReviewStatus.ReviewCount + 1
                    }
                },
                null
            );
        }

        // If we've completed all steps, graduate
        var nextStepProgress = step + 1;
        if (nextStepProgress >= RELEARNING_STEPS.Length)
        {
            const double graduatedInterval = 2.0;
            return (
                card with
                {
                    CardReviewStatus = card.CardReviewStatus with
                    {
                        LearningStep = null,
                        IntervalInDays = (int)Math.Round(graduatedInterval),
                        NextReviewDate = DateTime.UtcNow.AddDays(graduatedInterval),
                        ReviewCount = card.CardReviewStatus.ReviewCount + 1
                    }
                },
                ReviewPhase.Reviewing
            );
        }

        // Move to next relearning step
        var nextInterval = RELEARNING_STEPS[nextStepProgress];
        return (
            card with
            {
                CardReviewStatus = card.CardReviewStatus with
                {
                    LearningStep = nextStepProgress,
                    IntervalInDays = (int)Math.Round(nextInterval),
                    NextReviewDate = DateTime.UtcNow.AddDays(nextInterval),
                    ReviewCount = card.CardReviewStatus.ReviewCount + 1
                }
            },
            null
        );
    }
}



public class AnkiReviewStateMachine
{
    public Card Schedule(Card card, CardDifficulty difficulty, int repeatCount)
    {
        var currentPhase = card.CardReviewStatus.CurrentPhase ?? InferPhase(card);


        ICardReviewState state = currentPhase switch
        {
            ReviewPhase.Learning => new LearningState(),
            ReviewPhase.Relearning => new RelearningState(),
            ReviewPhase.Reviewing => new ReviewingState(),
            _ => throw new InvalidOperationException("Unknown review phase")
        };

        var (updatedCard, nextPhase) = state.Schedule(card, difficulty, repeatCount);

        if (nextPhase is not null && nextPhase != card.CardReviewStatus.CurrentPhase)
        {
            updatedCard = updatedCard with
            {
                CardReviewStatus = updatedCard.CardReviewStatus with
                {
                    CurrentPhase = nextPhase.Value
                }
            };
        }

        return updatedCard;
    }

    private ReviewPhase InferPhase(Card card)
    {
        var count = card.CardReviewStatus.ReviewCount;

        if (count <= 2)
            return ReviewPhase.Learning;

        return ReviewPhase.Reviewing;
    }
}