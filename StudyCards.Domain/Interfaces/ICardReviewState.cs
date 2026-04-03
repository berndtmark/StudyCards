using StudyCards.Domain.Enums;
using StudyCards.Domain.Study.State.AnkiState;
using StudyCards.Domain.ValueObjects;

namespace StudyCards.Domain.Interfaces;

public interface ICardReviewState
{
    (CardReviewStatus UpdatedCardStatus, ReviewPhase NextPhase) Schedule(CardReviewStatus cardStatus, CardReview[] pastCardReviews, CardDifficulty difficulty, int repeatCount, AnkiScheduleConfiguration configuration);
}