using Microsoft.Extensions.Logging;
using StudyCards.Application.Common;
using StudyCards.Application.Interfaces.CQRS;
using StudyCards.Application.Interfaces.UnitOfWork;

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
            var cards = await unitOfWork.CardRepository.GetByDeck(request.DeckId, cancellationToken);
            if (cards.Any())
            {
                unitOfWork.CardRepository.RemoveRange([.. cards]);
            }

            await unitOfWork.DeckRepository.Remove(request.DeckId, request.UserId);
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
