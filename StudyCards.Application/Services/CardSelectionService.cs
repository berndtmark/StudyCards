using StudyCards.Application.Interfaces;
using StudyCards.Domain.Entities;
using StudyCards.Domain.Enums;
using StudyCards.Domain.Interfaces;

namespace StudyCards.Application.Services;

public class CardSelectionService(IEnumerable<ICardsToStudyStrategy> cardStudyStrategies) : ICardSelectionService
{
    private readonly Dictionary<CardStudyMethodology, ICardsToStudyStrategy> strategyMap = cardStudyStrategies.ToDictionary(s => s.CardStudyMethodology, s => s);

    public IEnumerable<Card> SelectCards(CardStudyMethodology studyMethodology, IEnumerable<Card> cards, int noCardsToSelect, int noNewCardsToSelect)
    {
        if (!strategyMap.TryGetValue(studyMethodology, out var studyStrategy))
        {
            throw new ArgumentException("Methodology not yet supported ", nameof(studyMethodology));
        }

        if (!cards.Any() || noCardsToSelect == 0)
            return [];

        if (noNewCardsToSelect > noCardsToSelect)
            noNewCardsToSelect = noCardsToSelect;

        return studyStrategy.GetCards(cards, noCardsToSelect, noNewCardsToSelect);
    }
}
