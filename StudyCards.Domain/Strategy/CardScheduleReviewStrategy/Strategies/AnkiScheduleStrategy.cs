using StudyCards.Domain.Entities;
using StudyCards.Domain.Enums;
using StudyCards.Domain.Interfaces;
using StudyCards.Domain.State.AnkiState;

namespace StudyCards.Domain.Strategy.CardScheduleReviewStrategy.Strategies;

public class AnkiScheduleStrategy : ICardScheduleStrategy
{
    public Card ScheduleNext(Card card, CardDifficulty difficulty, int repeatCount = 0)
    {
        var ankiStateMachine = new AnkiStateMachine();
        return ankiStateMachine.Schedule(card, difficulty, repeatCount);
    }
}
