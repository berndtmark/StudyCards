using Microsoft.Extensions.Logging;
using StudyCards.Application.Common;
using StudyCards.Application.Exceptions;
using StudyCards.Application.Interfaces.CQRS;
using StudyCards.Application.Interfaces.UnitOfWork;
using StudyCards.Domain.Entities;

namespace StudyCards.Application.UseCases.DeckManagement.Commands;

public class RemoveDeckCommand : ICommand<bool>
{
    public Guid DeckId { get; set; }
    public Guid UserId { get; set; }
}

public class RemoveDeckCommandHandler(IUnitOfWork unitOfWork, ILogger<RemoveDeckCommand> logger) : ICommandHandler<RemoveDeckCommand, bool>
{
    public async Task<Result<bool>> Handle(RemoveDeckCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var deck = await unitOfWork.DeckRepository.Get(request.DeckId, request.UserId, cancellationToken) ?? throw new EntityNotFoundException(nameof(Deck), request.DeckId);

            var cards = await unitOfWork.CardRepository.GetByDeck(request.DeckId, cancellationToken);
            if (cards.Any())
            {
                unitOfWork.CardRepository.RemoveRange([.. cards]);
            }

            unitOfWork.DeckRepository.Remove(deck);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to remove deck {DeckId}", request.DeckId);
            throw;
        }
    }
}
