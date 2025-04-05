using StudyCards.Application.Enums;
using StudyCards.Application.Interfaces;
using StudyCards.Application.Strategy.CardStrategy.Strategies;

namespace StudyCards.Application.Factory;

public class CardSelectionStudyFactory : ICardSelectionStudyFactory
{
    public ICardStrategy Create(CardStudyMethodology studyMethodology)
    {
        return studyMethodology switch
        {
            CardStudyMethodology.Random => new RandomCardStrategy(),
            _ => throw new ArgumentException("Methodology not yet supported ", nameof(studyMethodology))
        };
    }
}
