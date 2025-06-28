using StudyCards.Domain.Entities;

namespace StudyCards.Domain.Interfaces;

public interface ICardsToStudyStrategy
{
    IEnumerable<Card> GetCards(IEnumerable<Card> cards, int noCardsToSelect, int noNewCardsToSelect);
}
