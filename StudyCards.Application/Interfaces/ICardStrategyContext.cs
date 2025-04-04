using StudyCards.Domain.Entities;

namespace StudyCards.Application.Interfaces;

public interface ICardStrategyContext
{
    IEnumerable<Card> GetCards(int noCardsToSelect);
}
