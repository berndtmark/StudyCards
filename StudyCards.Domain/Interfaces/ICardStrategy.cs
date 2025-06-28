using StudyCards.Domain.Entities;

namespace StudyCards.Domain.Interfaces;

public interface ICardStrategy
{
    IEnumerable<Card> GetCards(IEnumerable<Card> cards, int noCardsToSelect);
}
