using StudyCards.Domain.Entities;
using StudyCards.Domain.Enums;
using StudyCards.Domain.Interfaces;
using StudyCards.Domain.State.AnkiState.States;

namespace StudyCards.Domain.State.AnkiState;

public class AnkiStateMachine
{
    public Card Schedule(Card card, CardDifficulty difficulty, int repeatCount)
    {
        if (card == null)
            throw new ArgumentNullException(nameof(card));
        if (repeatCount < 0)
            throw new ArgumentException("Repeat count cannot be negative", nameof(repeatCount));

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

    // Used for existing cards that dont have phase set
    // Only a fallback to initially setup old cards
    private static ReviewPhase InferPhase(Card card)
    {
        var count = card.CardReviewStatus.ReviewCount;

        if (count <= 2)
            return ReviewPhase.Learning;

        return ReviewPhase.Reviewing;
    }
}
