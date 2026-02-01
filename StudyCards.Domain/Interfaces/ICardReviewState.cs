using StudyCards.Domain.Entities;
using StudyCards.Domain.Enums;
using StudyCards.Domain.Study.State.AnkiState;

namespace StudyCards.Domain.Interfaces;

public interface ICardReviewState
{
    (CardReviewStatus UpdatedCardStatus, ReviewPhase NextPhase) Schedule(CardReviewStatus cardStatus, CardReview[] pastCardReviews, CardDifficulty difficulty, int repeatCount, AnkiScheduleConfiguration configuration);
}