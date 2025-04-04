using StudyCards.Application.Enums;

namespace StudyCards.Application.Interfaces;

public interface ICardSelectionStudyFactory
{
    ICardStrategy CreateStudy(CardStudyMethodology studyMethodology);
}
