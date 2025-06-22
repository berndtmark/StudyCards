using StudyCards.Application.Interfaces;
using StudyCards.Application.Interfaces.UnitOfWork;
using StudyCards.Domain.Entities;
using StudyCards.Domain.Enums;
using StudyCards.Domain.Extensions;

namespace StudyCards.Application.UseCases.CardStudy.Review;

public class ReviewCardsUseCaseRequest
{
    public Guid DeckId { get; set; }
    public IList<CardReviewed> CardReviews { get; set; } = new List<CardReviewed>();

    public class CardReviewed
    {   
        public Guid CardId { get; set; }
        public CardDifficulty CardDifficulty { get; set; }
        public int? RepeatCount { get; set; }
    }
}

public class ReviewCardsUseCards(IUnitOfWork unitOfWork) : IUseCase<ReviewCardsUseCaseRequest, IList<Card>>
{
    public async Task<IList<Card>> Handle(ReviewCardsUseCaseRequest request)
    {
        var response = new List<Card>();

        // update card reviews
        foreach (var cardReview in request.CardReviews)
        {
            var card = await unitOfWork.CardRepository.Get(cardReview.CardId, request.DeckId) ?? throw new Exception("Card not found");

            var review = new CardReview
            {
                CardReviewId = Guid.NewGuid(),
                CardDifficulty = cardReview.CardDifficulty,
                RepeatCount = cardReview.RepeatCount ?? 0,
                ReviewDate = DateTime.UtcNow
            };

            var updatedCard = card with
            {
                CardReviews = [.. card.CardReviews.Prepend(review).Take(10)] // ideally we should keep reviews in another container, but ja, costs. So limiting to 10 reviews, its all we need for now
            };

            var result = unitOfWork.CardRepository.Update(updatedCard);
            response.Add(result);
        }

        // update deck last review status
        var deck = await unitOfWork.DeckRepository.Get(request.DeckId) ?? throw new Exception($"Deck not found ${request.DeckId}");

        var isFirstReviewToday = !deck.DeckReviewStatus.LastReview.Date.IsSameDay();
        var updatedDeck = deck with
        {
            DeckReviewStatus = new DeckReviewStatus
            {
                LastReview = DateTime.UtcNow,
                ReviewCount = isFirstReviewToday ? request.CardReviews.Count : deck.DeckReviewStatus.ReviewCount + request.CardReviews.Count,
            }
        };

        unitOfWork.DeckRepository.Update(updatedDeck);

        await unitOfWork.SaveChangesAsync();
        return response;
    }
}
