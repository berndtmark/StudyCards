using Microsoft.Extensions.Logging;
using StudyCards.Application.Interfaces;
using StudyCards.Application.Interfaces.Repositories;
using StudyCards.Domain.Entities;

namespace StudyCards.Application.UseCases.DeckManagement.AddDeck;

public class AddDeckUseCaseRequest
{
    public string DeckName { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
}
public class AddDeckUseCase(IDeckRepository deckRepository, ILogger<AddDeckUseCase> logger) : IUseCase<AddDeckUseCaseRequest, Deck>
{
    public async Task<Deck> Handle(AddDeckUseCaseRequest request)
    {
        var deck = new Deck
        {
            Id = Guid.NewGuid(),
            DeckName = request.DeckName,
            UserEmail = request.EmailAddress
        };

        try
        {
            var result = await deckRepository.Add(deck);
            return result;
        }
        catch (Exception)
        {
            logger.LogError("Failed to add deck {DeckName}", request.DeckName);
            return default!;
        }
    }
}
