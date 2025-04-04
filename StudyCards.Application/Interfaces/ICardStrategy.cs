using StudyCards.Domain.Entities;

namespace StudyCards.Application.Interfaces;

public interface ICardStrategy
{
    IEnumerable<Card> GetCards(IEnumerable<Card> cards, int noCardsToSelect);
}
