using StudyCards.Application.Interfaces.Repositories;
using StudyCards.Application.Interfaces;
using StudyCards.Domain.Entities;

namespace StudyCards.Application.UseCases.DeckManagement.GetDeck;

public class GetDeckRequest
{
    public string EmailAddress { get; set; } = string.Empty;
}

public class GetDeckUseCase(IDeckRepository deckRepository) : IUseCase<GetDeckRequest, IEnumerable<Deck>>
{
    public async Task<IEnumerable<Deck>> Handle(GetDeckRequest request)
    {
        return await deckRepository.GetByEmail(request.EmailAddress);
    }
}
