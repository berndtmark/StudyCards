using StudyCards.Domain.Entities;
using StudyCards.Domain.Enums;
using StudyCards.Domain.State.AnkiState;

namespace StudyCards.Domain.Interfaces;

public interface ICardReviewState
{
    (CardReviewStatus UpdatedCardStatus, ReviewPhase NextPhase) Schedule(CardReviewStatus cardStatus, CardDifficulty difficulty, int repeatCount, AnkiScheduleConfiguration configuration);
}