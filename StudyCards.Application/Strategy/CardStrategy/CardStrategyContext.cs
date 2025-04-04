using StudyCards.Application.Interfaces;
using StudyCards.Domain.Entities;

namespace StudyCards.Application.Strategy.CardStrategy;

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

        if (!_cards.Any())
            throw new ArgumentException("Cards collection cannot be null or empty.");

        if (noCardsToSelect <= 0)
            throw new ArgumentException("The number of cards to select must be greater than zero.");

        return _strategy.GetCards(_cards, noCardsToSelect);
    }
}
