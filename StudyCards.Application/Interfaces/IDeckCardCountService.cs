using StudyCards.Application.Interfaces.UnitOfWork;

namespace StudyCards.Application.Interfaces;

public interface IDeckCardCountService
{
    Task UpdateDeckCardCount(Guid deckId, IUnitOfWork unitOfWork, int incrementValue = 0);
}
