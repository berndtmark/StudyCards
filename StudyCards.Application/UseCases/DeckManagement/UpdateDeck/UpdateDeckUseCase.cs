using StudyCards.Application.Interfaces;
using StudyCards.Application.Interfaces.UnitOfWork;
using StudyCards.Domain.Entities;

namespace StudyCards.Application.UseCases.DeckManagement.UpdateDeck;

public class UpdateDeckUseCaseRequest
{
    public Guid DeckId { get; set; }
    public string DeckName { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public int ReviewsPerDay { get; set; }
    public int NewCardsPerDay { get; set; }
}

public class UpdateDeckUseCase(IUnitOfWork unitOfWork) : IUseCase<UpdateDeckUseCaseRequest, Deck>
{
    public async Task<Deck> Handle(UpdateDeckUseCaseRequest request)
    {
        var currentDeck = await unitOfWork.DeckRepository.Get(request.DeckId, request.EmailAddress);

        if (currentDeck == null)
        {
            throw new Exception("Deck not found");
        }

        var newDeck = currentDeck with
        {
            DeckName = request.DeckName,
            Description = request.Description,
            UserEmail = request.EmailAddress,
            DeckSettings = currentDeck.DeckSettings with
            {
                NewCardsPerDay = request.NewCardsPerDay,
                ReviewsPerDay = request.ReviewsPerDay,
            }
        };

        var result = unitOfWork.DeckRepository.Update(newDeck);
        await unitOfWork.SaveChangesAsync();

        return result;
    }
}
