using StudyCards.Domain.DomainEvents;
using StudyCards.Domain.Enums;
using StudyCards.Domain.ValueObjects;

namespace StudyCards.Domain.Entities;

public record Card : EntityBase
{
    public Guid DeckId { get; init; }
    public string CardFront { get; init; } = string.Empty;
    public string CardBack { get; init; } = string.Empty;
    public ICollection<CardReview> CardReviews { get; init; } = [];
    public CardReviewStatus CardReviewStatus { get; init; } = new();

    #region Behaviours
    public static Card Create(Guid Deck, string cardFront, string cardBack)
    {
        return new Card 
        { 
            Id = Guid.NewGuid(), 
            DeckId = Deck, 
            CardFront = cardFront, 
            CardBack = cardBack 
        };
    }

    public Card Update(string cardFront, string cardBack)
    {
        return this with
        {
            CardFront = cardFront,
            CardBack = cardBack
        };
    }

    public Card AddReview(CardReview newReview, int maxReviewsToKeep = 10)
    {
        return this with
        {
            CardReviews = [.. CardReviews.Prepend(newReview).Take(maxReviewsToKeep)]
        };
    }

    public Card AddCardReviewStatus(CardReviewStatus updatedCardStatus, ReviewPhase nextPhase)
    {
        var result = this with
        {
            CardReviewStatus = updatedCardStatus with
            {
                CurrentPhase = nextPhase,
                ReviewCount = updatedCardStatus.ReviewCount + 1,
            }
        };

        result.Raise(new CardReviewedDomainEvent(Id, CardReviewStatus.CurrentPhase, result.CardReviewStatus));
        return result;
    }
    #endregion Behaviours
}
