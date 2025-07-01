using StudyCards.Domain.Entities;

namespace StudyCards.Domain.Interfaces;

public interface ICardsToStudyStrategyContext
{
    void SetStrategy(ICardsToStudyStrategy strategy);
    void AddCards(IEnumerable<Card> cards);
    IEnumerable<Card> GetStudyCards(int noCardsToSelect, int noNewCardsToSelect);
}
