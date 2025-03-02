using StudyCards.Application.Interfaces;
using StudyCards.Data.Interfaces;

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
