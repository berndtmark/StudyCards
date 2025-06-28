using StudyCards.Domain.Entities;

namespace StudyCards.Domain.Interfaces;

public interface ICardStrategyContext
{
    void SetStrategy(ICardStrategy strategy);
    void AddCards(IEnumerable<Card> cards);
    IEnumerable<Card> GetCards(int noCardsToSelect);
}
