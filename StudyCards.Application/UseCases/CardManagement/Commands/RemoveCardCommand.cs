using Microsoft.Extensions.Logging;
using StudyCards.Application.Interfaces;
using StudyCards.Application.Interfaces.CQRS;
using StudyCards.Application.Interfaces.UnitOfWork;

namespace StudyCards.Application.UseCases.CardManagement.Commands;

public class RemoveCardCommand : ICommand<bool>
{
    public Guid CardId { get; set; }
    public Guid DeckId { get; set; }
}

public class RemoveCardCommandHandler(IUnitOfWork unitOfWork, ILogger<RemoveCardCommand> logger, IDeckCardCountService deckCardCount) : ICommandHandler<RemoveCardCommand, bool>
{
    public async Task<bool> Handle(RemoveCardCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await unitOfWork.CardRepository.Remove(request.CardId, request.DeckId);
            await deckCardCount.UpdateDeckCardCount(request.DeckId, unitOfWork, -1);

            await unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to remove card {CardId}", request.CardId);
            throw;
        }
    }
}
