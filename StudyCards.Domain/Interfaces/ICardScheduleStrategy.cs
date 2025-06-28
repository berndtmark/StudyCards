using StudyCards.Domain.Entities;
using StudyCards.Domain.Enums;

namespace StudyCards.Domain.Interfaces;

public interface ICardScheduleStrategy
{
    Card ScheduleNext(Card card, CardDifficulty difficulty, int repeatCount = 0);
}
