using StudyCards.Application.Common;
using StudyCards.Application.Exceptions;
using StudyCards.Application.Interfaces.CQRS;
using StudyCards.Application.Interfaces.UnitOfWork;
using StudyCards.Domain.Entities;

namespace StudyCards.Application.UseCases.DeckManagement.Commands;

public class UpdateDeckCommand : ICommand<Deck>
{
    public Guid DeckId { get; set; }
    public Guid UserId { get; set; }
    public string DeckName { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public int ReviewsPerDay { get; set; }
    public int NewCardsPerDay { get; set; }
}

public class UpdateDeckCommandHandler(IUnitOfWork unitOfWork) : ICommandHandler<UpdateDeckCommand, Deck>
{
    public async Task<Result<Deck>> Handle(UpdateDeckCommand request, CancellationToken cancellationToken)
    {
        var currentDeck = await unitOfWork.DeckRepository.Get(request.DeckId, request.UserId, cancellationToken) ?? throw new EntityNotFoundException(nameof(Deck), request.DeckId);

        var deckSettings = new DeckSettings
        {
            NewCardsPerDay = request.NewCardsPerDay,
            ReviewsPerDay = request.ReviewsPerDay
        };

        var newDeck = currentDeck.Update(request.DeckName, request.Description, deckSettings);

        var result = unitOfWork.DeckRepository.Update(newDeck);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return result;
    }
}
