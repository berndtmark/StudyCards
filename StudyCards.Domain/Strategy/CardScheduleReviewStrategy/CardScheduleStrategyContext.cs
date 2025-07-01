using StudyCards.Domain.Entities;
using StudyCards.Domain.Interfaces;

namespace StudyCards.Domain.Strategy.CardScheduleReviewStrategy;

public class CardScheduleStrategyContext : ICardScheduleStrategyContext
{
    private ICardScheduleStrategy _strategy = default!;
    private IEnumerable<Card> _cards = default!;

    public void SetStrategy(ICardScheduleStrategy strategy)
    {
        _strategy = strategy;
    }

    public void AddCards(IEnumerable<Card> cards)
    {
        _cards = cards;
    }

    public IEnumerable<Card> ScheduleCards(IEnumerable<CardSchedule> cards)
    {
        if (_strategy == null)
        {
            throw new InvalidOperationException("Strategy not set.");
        }

        if (!_cards.Any())
            return [];

        var scheduledCards = cards.Select(c => _strategy.ScheduleNext(c.Card, c.Difficulty, c.RepeatCount));
        return scheduledCards;
    }
}
