using Microsoft.Extensions.Logging;
using StudyCards.Application.Common;
using StudyCards.Application.Exceptions;
using StudyCards.Application.Interfaces.CQRS;
using StudyCards.Application.Interfaces.UnitOfWork;
using StudyCards.Domain.Entities;

namespace StudyCards.Application.UseCases.CardManagement.Commands;

public class AddCardsCommand : ICommand<AddCardsCommandResponse>
{
    public Guid DeckId { get; set; }
    public IList<(string CardFront, string CardBack)> Cards { get; set; } = [];
}

public record AddCardsCommandResponse(IList<Card> CardsAdded, IList<Card> CardsSkipped);

public class AddCardsCommandHandler(IUnitOfWork unitOfWork, ILogger<AddCardsCommand> logger) : ICommandHandler<AddCardsCommand, AddCardsCommandResponse>
{
    public async Task<Result<AddCardsCommandResponse>> Handle(AddCardsCommand request, CancellationToken cancellationToken)
    {
        var deck = await unitOfWork.DeckRepository.Get(request.DeckId, cancellationToken) ?? throw new EntityNotFoundException(nameof(Deck), request.DeckId);

        var currentCardsInDeck = await unitOfWork.CardRepository.GetByDeck(request.DeckId, cancellationToken);

        var cardsToAdd = new List<Card>();
        var cardsSkipped = new List<Card>();

        foreach (var (CardFront, CardBack) in request.Cards)
        {
            var duplicate = currentCardsInDeck.FirstOrDefault(c => c.CardFront == CardFront && c.CardBack == CardBack);

            if (duplicate != null)
            {
                cardsSkipped.Add(duplicate);
                continue;
            }

            var newCard = Card.Create(request.DeckId, CardFront, CardBack);
            cardsToAdd.Add(newCard);
        }

        try
        {
            if (cardsToAdd.Count != 0)
            {
                await unitOfWork.CardRepository.Add(cardsToAdd);

                // update check card count
                deck.UpdateCardCount(currentCardsInDeck.Count() + cardsToAdd.Count);
                unitOfWork.DeckRepository.Update(deck);

                await unitOfWork.SaveChangesAsync(cancellationToken);
            }            

            return new AddCardsCommandResponse([.. cardsToAdd], [.. cardsSkipped]);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to add cards to deck {DeckId}", request.DeckId);
            throw;
        }
    }
}
