using Microsoft.Extensions.Logging;
using StudyCards.Application.Interfaces;
using StudyCards.Application.Interfaces.CQRS;
using StudyCards.Application.Interfaces.UnitOfWork;
using StudyCards.Domain.Entities;

namespace StudyCards.Application.UseCases.CardManagement.Commands;

public class AddCardCommand : ICommand<Card>
{
    public Guid DeckId { get; set; }
    public string CardFront { get; set; } = string.Empty;
    public string CardBack { get; set; } = string.Empty;
}

public class AddCardCommandHandler(IUnitOfWork unitOfWork, ILogger<AddCardCommand> logger, IDeckCardCountService deckCardCount) : ICommandHandler<AddCardCommand, Card>
{
    public async Task<Card> Handle(AddCardCommand request, CancellationToken cancellationToken)
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
            var result = await unitOfWork.CardRepository.Add(card);
            await deckCardCount.UpdateDeckCardCount(request.DeckId, unitOfWork, 1, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to add card to deck {DeckId}", request.DeckId);
            throw;
        }
    }
}
