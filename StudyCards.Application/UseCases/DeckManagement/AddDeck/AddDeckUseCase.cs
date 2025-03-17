using Microsoft.Extensions.Logging;
using StudyCards.Application.Interfaces;
using StudyCards.Application.Interfaces.Repositories;
using StudyCards.Domain.Entities;

namespace StudyCards.Application.UseCases.DeckManagement.AddDeck;

public class AddDeckUseCaseRequest
{
    public string DeckName { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int ReviewsPerDay { get; set; }
    public int NewCardsPerDay { get; set; }
}
public class AddDeckUseCase(IDeckRepository deckRepository, ILogger<AddDeckUseCase> logger) : IUseCase<AddDeckUseCaseRequest, Deck>
{
    public async Task<Deck> Handle(AddDeckUseCaseRequest request)
    {
        var deck = new Deck
        {
            Id = Guid.NewGuid(),
            DeckName = request.DeckName,
            Description = request.Description,
            UserEmail = request.EmailAddress,
            DeckSettings = new DeckSettings
            {
                NewCardsPerDay = request.NewCardsPerDay,
                ReviewsPerDay = request.ReviewsPerDay,
            }
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
