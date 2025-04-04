using StudyCards.Application.Enums;
using StudyCards.Application.Interfaces;
using StudyCards.Application.Strategy.CardStrategy.Strategies;

namespace StudyCards.Application.Factory;

public class CardSelectionStudyFactory : ICardSelectionStudyFactory
{
    public ICardStrategy CreateStudy(CardStudyMethodology studyMethodology)
    {
        return studyMethodology switch
        {
            CardStudyMethodology.Random => new RandomCardStrategy(),
            _ => throw new ArgumentException("Invalid methodology ", nameof(studyMethodology))
        };
    }
}
