using StudyCards.Application.Interfaces;
using StudyCards.Application.Interfaces.Repositories;

namespace StudyCards.Application.UseCases.CardManagement.AddCard;

public class AddCardRequest
{
}

public class AddCardUseCase(ICardRepository cardRepository) : IUseCase<AddCardRequest, bool>
{
    public Task<bool> Handle(AddCardRequest request)
    {
        throw new NotImplementedException();
    }
}
