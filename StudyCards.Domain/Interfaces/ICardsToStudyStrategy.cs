using StudyCards.Domain.Entities;
using StudyCards.Domain.Enums;

namespace StudyCards.Domain.Interfaces;

public interface ICardsToStudyStrategy
{
    CardStudyMethodology CardStudyMethodology { get; }
    IEnumerable<Card> GetCards(IEnumerable<Card> cards, int noCardsToSelect, int noNewCardsToSelect);
}
