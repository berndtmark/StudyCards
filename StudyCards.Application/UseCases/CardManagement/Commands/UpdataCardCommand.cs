using StudyCards.Application.Common;
using StudyCards.Application.Exceptions;
using StudyCards.Application.Interfaces.CQRS;
using StudyCards.Application.Interfaces.UnitOfWork;
using StudyCards.Domain.Entities;

namespace StudyCards.Application.UseCases.CardManagement.Commands;

public class UpdateCardCommand : ICommand<Card>
{
    public Guid CardId { get; set; }
    public Guid DeckId { get; set; }
    public string CardFront { get; set; } = string.Empty;
    public string CardBack { get; set; } = string.Empty;
}

public class UpdataCardCommandHandler(IUnitOfWork unitOfWork) : ICommandHandler<UpdateCardCommand, Card>
{
    public async Task<Result<Card>> Handle(UpdateCardCommand request, CancellationToken cancellationToken)
    {
        var currentCard = await unitOfWork.CardRepository.Get(request.CardId, request.DeckId, cancellationToken) ?? throw new EntityNotFoundException(nameof(Card), request.CardId);
        var newCard = currentCard with
        {
            CardFront = request.CardFront,
            CardBack = request.CardBack
        };

        var result = unitOfWork.CardRepository.Update(newCard);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return result;
    }
}