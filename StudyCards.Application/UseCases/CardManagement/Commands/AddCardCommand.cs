using Microsoft.Extensions.Logging;
using StudyCards.Application.Common;
using StudyCards.Application.Interfaces;
using StudyCards.Application.Interfaces.CQRS;
using StudyCards.Application.Interfaces.UnitOfWork;
using StudyCards.Domain.Entities;

namespace StudyCards.Application.UseCases.CardManagement.Commands;

public class AddCardCommand : ICommand<Result<Card>>
{
    public Guid DeckId { get; set; }
    public string CardFront { get; set; } = string.Empty;
    public string CardBack { get; set; } = string.Empty;
}

public class AddCardCommandHandler(IUnitOfWork unitOfWork, ILogger<AddCardCommand> logger, IDeckCardCountService deckCardCount) : ICommandHandler<AddCardCommand, Result<Card>>
{
    public async Task<Result<Card>> Handle(AddCardCommand request, CancellationToken cancellationToken)
    {
        var card = new Card
        {
            Id = Guid.NewGuid(),
            DeckId = request.DeckId,
            CardFront = request.CardFront,
            CardBack = request.CardBack
        };

        try
        {
            // Check if the card already exists in the deck
            var isExisting = await ValidateExisting(card, cancellationToken);
            if (isExisting)
                return Result<Card>.Failure("Card is already in the Deck");

            // Add the card to the repository
            var result = await unitOfWork.CardRepository.Add(card);
            await deckCardCount.UpdateDeckCardCount(request.DeckId, unitOfWork, 1, cancellationToken);

            // Save and return the result
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to add card to deck {DeckId}", request.DeckId);
            throw;
        }
    }

    private async Task<bool> ValidateExisting(Card card, CancellationToken cancellationToken)
    {
        var existingCard = await unitOfWork.CardRepository.Search(card.DeckId, card.CardFront, cancellationToken);

        return existingCard?.Any() ?? false;
    }
}
