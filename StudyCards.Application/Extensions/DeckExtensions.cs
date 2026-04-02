using StudyCards.Application.Exceptions;
using StudyCards.Application.Interfaces.Repositories;
using StudyCards.Application.Interfaces.UnitOfWork;
using StudyCards.Domain.Entities;

namespace StudyCards.Application.Extensions;

public static class DeckExtensions
{
    public static async Task EnsureOwnership(this IDeckRepository repository, Guid deckId, CancellationToken cancellationToken = default)
    {
        if (!await repository.IsOwner(deckId, cancellationToken))
        {
            throw new EntityNotFoundException(nameof(Deck), deckId);
        }
    }

    public static async Task SyncCardCount(this Deck deck, IUnitOfWork unitOfWork, int incrementValue = 0, CancellationToken cancellationToken = default)
    {
        var cardCount = await unitOfWork.CardRepository.CountByDeck(deck.Id, cancellationToken);

        deck.UpdateCardCount(cardCount + incrementValue);
        unitOfWork.DeckRepository.Update(deck);
    }
}
