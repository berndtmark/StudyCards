using StudyCards.Application.Interfaces;
using StudyCards.Domain.Enums;
using StudyCards.Domain.Interfaces;
using StudyCards.Domain.Study.Strategy.CardsToStudyStrategy.Strategies;

namespace StudyCards.Application.Factory;

public class CardSelectionStudyFactory : ICardSelectionStudyFactory
{
    public ICardsToStudyStrategy Create(CardStudyMethodology studyMethodology)
    {
        return studyMethodology switch
        {
            CardStudyMethodology.Random => new RandomCardStrategy(),
            CardStudyMethodology.Anki => new AnkiCardStrategy(),
            _ => throw new ArgumentException("Methodology not yet supported ", nameof(studyMethodology))
        };
    }
}
