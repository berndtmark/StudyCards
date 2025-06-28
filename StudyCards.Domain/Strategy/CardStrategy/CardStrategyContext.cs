using StudyCards.Domain.Entities;
using StudyCards.Domain.Interfaces;

namespace StudyCards.Domain.Strategy.CardStrategy;

public class CardStrategyContext : ICardStrategyContext
{
    private ICardStrategy _strategy = default!;
    private IEnumerable<Card> _cards = default!;

    public void SetStrategy(ICardStrategy strategy)
    {
        _strategy = strategy;
    }

    public void AddCards(IEnumerable<Card> cards)
    {
        _cards = cards;
    }

    public IEnumerable<Card> GetCards(int noCardsToSelect)
    {
        if (_strategy == null)
        {
            throw new InvalidOperationException("Strategy not set.");
        }

        if (!_cards.Any() || noCardsToSelect == 0)
            return [];

        return _strategy.GetCards(_cards, noCardsToSelect);
    }
}
