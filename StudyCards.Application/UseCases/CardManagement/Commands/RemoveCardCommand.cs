using Microsoft.Extensions.Logging;
using StudyCards.Application.Common;
using StudyCards.Application.Exceptions;
using StudyCards.Application.Extensions;
using StudyCards.Application.Interfaces.CQRS;
using StudyCards.Application.Interfaces.UnitOfWork;
using StudyCards.Domain.Entities;

namespace StudyCards.Application.UseCases.CardManagement.Commands;

public class RemoveCardCommand : ICommand<bool>
{
    public Guid CardId { get; set; }
    public Guid DeckId { get; set; }
}

public class RemoveCardCommandHandler(IUnitOfWork unitOfWork, ILogger<RemoveCardCommand> logger) : ICommandHandler<RemoveCardCommand, bool>
{
    public async Task<Result<bool>> Handle(RemoveCardCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var deck = await unitOfWork.DeckRepository.Get(request.DeckId, cancellationToken) ?? throw new EntityNotFoundException(nameof(Deck), request.DeckId);

            await unitOfWork.CardRepository.Remove(request.DeckId, request.CardId);
            await deck.SyncCardCount(unitOfWork, -1, cancellationToken);

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
