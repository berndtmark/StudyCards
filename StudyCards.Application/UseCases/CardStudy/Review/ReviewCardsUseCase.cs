using StudyCards.Application.Interfaces;
using StudyCards.Application.Interfaces.UnitOfWork;
using StudyCards.Domain.Entities;
using StudyCards.Domain.Enums;

namespace StudyCards.Application.UseCases.CardStudy.Review;

public class ReviewCardsUseCaseRequest
{
    public Guid DeckId { get; set; }
    public IList<CardReviewed> CardReviews { get; set; } = new List<CardReviewed>();

    public class CardReviewed
    {   
        public Guid CardId { get; set; }
        public CardDifficulty CardDifficulty { get; set; }
    }
}

public class ReviewCardsUseCards(IUnitOfWork unitOfWork) : IUseCase<ReviewCardsUseCaseRequest, IList<Card>>
{
    public async Task<IList<Card>> Handle(ReviewCardsUseCaseRequest request)
    {
        var response = new List<Card>();

        foreach (var cardReview in request.CardReviews)
        {
            var card = await unitOfWork.CardRepository.Get(cardReview.CardId, request.DeckId);
            if (card == null)
            {
                throw new Exception("Card not found");
            }

            var review = new CardReview
            {
                CardReviewId = Guid.NewGuid(),
                CardDifficulty = cardReview.CardDifficulty,
                ReviewDate = DateTime.UtcNow
            };

            var updatedCard = card with
            {
                CardReviews = [.. card.CardReviews, review]
            };

            var result = unitOfWork.CardRepository.Update(updatedCard);
            response.Add(result);
        }

        await unitOfWork.SaveChangesAsync();
        return response;
    }
}
