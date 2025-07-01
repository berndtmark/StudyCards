using StudyCards.Domain.Entities;
using StudyCards.Domain.Strategy.CardScheduleReviewStrategy;

namespace StudyCards.Domain.Interfaces;

public interface ICardScheduleStrategyContext
{
    void SetStrategy(ICardScheduleStrategy strategy);
    void AddCards(IEnumerable<Card> cards);
    IEnumerable<Card> ScheduleCards(IEnumerable<CardSchedule> cards);
}
