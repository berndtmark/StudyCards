using StudyCards.Domain.Entities;
using StudyCards.Domain.Interfaces;

namespace StudyCards.Domain.Study.Strategy.CardsToStudyStrategy;

public class CardsToStudyStrategyContext : ICardsToStudyStrategyContext
{
    private ICardsToStudyStrategy _strategy = default!;
    private IEnumerable<Card> _cards = default!;

    public void SetStrategy(ICardsToStudyStrategy strategy)
    {
        _strategy = strategy;
    }

    public void AddCards(IEnumerable<Card> cards)
    {
        _cards = cards;
    }

    public IEnumerable<Card> GetStudyCards(int noCardsToSelect, int noNewCardsToSelect)
    {
        if (_strategy == null)
            throw new InvalidOperationException("Strategy not set.");

        if (!_cards.Any() || noCardsToSelect == 0)
            return [];

        if (noNewCardsToSelect > noCardsToSelect)
            noNewCardsToSelect = noCardsToSelect;

        return _strategy.GetCards(_cards, noCardsToSelect, noNewCardsToSelect);
    }
}
