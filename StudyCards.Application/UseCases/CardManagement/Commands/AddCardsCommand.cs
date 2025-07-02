using Microsoft.Extensions.Logging;
using StudyCards.Application.Interfaces;
using StudyCards.Application.Interfaces.CQRS;
using StudyCards.Application.Interfaces.UnitOfWork;
using StudyCards.Domain.Entities;
using System.Collections.Concurrent;

namespace StudyCards.Application.UseCases.CardManagement.Commands;

public class AddCardsCommand : ICommand<AddCardsCommandResponse>
{
    public Guid DeckId { get; set; }
    public IList<(string CardFront, string CardBack)> Cards { get; set; } = [];
}

public record AddCardsCommandResponse(IList<Card> CardsAdded, IList<Card> CardsSkipped);

public class AddCardsCommandHandler(IUnitOfWork unitOfWork, ILogger<AddCardsCommand> logger, IDeckCardCountService deckCardCount) : ICommandHandler<AddCardsCommand, AddCardsCommandResponse>
{
    public async Task<AddCardsCommandResponse> Handle(AddCardsCommand request, CancellationToken cancellationToken)
    {
        var currentCardsInDeck = await unitOfWork.CardRepository.GetByDeck(request.DeckId, cancellationToken);

        var cardsAdded = new ConcurrentBag<Card>();
        var cardsSkipped = new ConcurrentBag<Card>();

        var tasks = request.Cards.Select(async card =>
        {
            var duplicate = currentCardsInDeck.FirstOrDefault(c => c.CardFront == card.CardFront && c.CardBack == card.CardBack);

            if (duplicate != null)
            {
                cardsSkipped.Add(duplicate);
                return;
            }

            var newCard = new Card
            {
                Id = Guid.NewGuid(),
                DeckId = request.DeckId,
                CardFront = card.CardFront,
                CardBack = card.CardBack
            };

            var addResult = await unitOfWork.CardRepository.Add(newCard);
            cardsAdded.Add(addResult);
        });

        try
        {
            await Task.WhenAll(tasks);
            await deckCardCount.UpdateDeckCardCount(request.DeckId, unitOfWork, cardsAdded.Count, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);
            return new AddCardsCommandResponse(cardsAdded.ToList(), cardsSkipped.ToList());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to add cards to deck {DeckId}", request.DeckId);
            throw;
        }
    }
}
