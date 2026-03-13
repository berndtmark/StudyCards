using StudyCards.Domain.Entities;
using StudyCards.Domain.Enums;

namespace StudyCards.Application.Interfaces;

public interface ICardSelectionService
{
    IEnumerable<Card> SelectCards(CardStudyMethodology studyMethodology, IEnumerable<Card> cards, int noCardsToSelect, int noOfNewCardsToSelect);
}
