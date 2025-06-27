using Microsoft.Extensions.Logging;
using StudyCards.Application.Interfaces.CQRS;
using StudyCards.Application.Interfaces.UnitOfWork;

namespace StudyCards.Application.UseCases.DeckManagement.Commands;

public class RemoveDeckCommand : ICommand<bool>
{
    public Guid DeckId { get; set; }
    public string EmailAddress { get; set; } = string.Empty;
}

public class RemoveDeckCommandHandler(IUnitOfWork unitOfWork, ILogger<RemoveDeckCommand> logger) : ICommandHandler<RemoveDeckCommand, bool>
{
    public async Task<bool> Handle(RemoveDeckCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var cards = await unitOfWork.CardRepository.GetByDeck(request.DeckId, cancellationToken);
            if (cards.Any())
            {
                unitOfWork.CardRepository.RemoveRange([.. cards]);
            }

            await unitOfWork.DeckRepository.Remove(request.DeckId, request.EmailAddress);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
        catch (Exception)
        {
            logger.LogError("Failed to remove deck {DeckId}", request.DeckId);
            return false;
        }
    }
}
