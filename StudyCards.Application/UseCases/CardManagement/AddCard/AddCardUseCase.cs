using StudyCards.Application.Interfaces;
using StudyCards.Application.Interfaces.Repositories;

namespace StudyCards.Application.UseCases.CardManagement.AddCard;

public class AddCardUseCase(ICardRepository cardRepository) : IUseCase<AddCardRequest, string>
{
    public Task<string> Handle()
    {
        throw new NotImplementedException();
    }
}

// todo move
public class AddCardRequest
{
}
