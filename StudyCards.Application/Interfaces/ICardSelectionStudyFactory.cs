using StudyCards.Domain.Enums;
using StudyCards.Domain.Interfaces;

namespace StudyCards.Application.Interfaces;

public interface ICardSelectionStudyFactory
{
    ICardStrategy Create(CardStudyMethodology studyMethodology);
}
