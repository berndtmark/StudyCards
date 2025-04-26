using StudyCards.Application.Interfaces;
using StudyCards.Application.Interfaces.UnitOfWork;
using StudyCards.Domain.Entities;

namespace StudyCards.Application.UseCases.CardManagement.UpdateCard;

public class UpdateCardUseCaseRequest
{
    public Guid CardId { get; set; }
    public Guid DeckId { get; set; }
    public string CardFront { get; set; } = string.Empty;
    public string CardBack { get; set; } = string.Empty;
}

public class UpdataCardUseCase(IUnitOfWork unitOfWork) : IUseCase<UpdateCardUseCaseRequest, Card>
{
    public async Task<Card> Handle(UpdateCardUseCaseRequest request)
    {
        var currentCard = await unitOfWork.CardRepository.Get(request.CardId, request.DeckId);

        if (currentCard == null)
        {
            throw new Exception("Card not found");
        }

        var newCard = currentCard with
        {
            CardFront = request.CardFront,
            CardBack = request.CardBack
        };

        var result = unitOfWork.CardRepository.Update(newCard);
        await unitOfWork.SaveChangesAsync();

        return result;
    }
}