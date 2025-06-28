using StudyCards.Domain.Entities;
using StudyCards.Domain.Interfaces;

namespace StudyCards.Domain.Strategy.CardStrategy.Strategies;

public class RandomCardStrategy : ICardStrategy
{
    public IEnumerable<Card> GetCards(IEnumerable<Card> cards, int noCardsToSelect)
    {
        Random random = new();
        return cards.OrderBy(x => random.Next()).Take(noCardsToSelect);
    }
}
