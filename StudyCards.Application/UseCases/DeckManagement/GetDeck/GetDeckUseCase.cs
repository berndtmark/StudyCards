using StudyCards.Application.Interfaces.Repositories;
using StudyCards.Application.Interfaces;
using StudyCards.Domain.Entities;

namespace StudyCards.Application.UseCases.DeckManagement.GetDeck;

public class GetDeckUseCaseRequest
{
    public string EmailAddress { get; set; } = string.Empty;
}

public class GetDeckUseCase(IDeckRepository deckRepository) : IUseCase<GetDeckUseCaseRequest, IEnumerable<Deck>>
{
    public async Task<IEnumerable<Deck>> Handle(GetDeckUseCaseRequest request)
    {
        return await deckRepository.GetByEmail(request.EmailAddress);
    }
}
