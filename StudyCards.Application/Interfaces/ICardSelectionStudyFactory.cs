using StudyCards.Application.Enums;

namespace StudyCards.Application.Interfaces;

public interface ICardSelectionStudyFactory
{
    ICardStrategy Create(CardStudyMethodology studyMethodology);
}
