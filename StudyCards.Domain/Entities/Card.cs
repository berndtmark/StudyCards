using StudyCards.Domain.DomainEvents;
using StudyCards.Domain.Enums;

namespace StudyCards.Domain.Entities;

public record Card : EntityBase
{
    public Guid DeckId { get; init; }
    public string CardFront { get; init; } = string.Empty;
    public string CardBack { get; init; } = string.Empty;
    public ICollection<CardReview> CardReviews { get; init; } = [];
    public CardReviewStatus CardReviewStatus { get; init; } = new();

    #region Behaviours
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
    #endregion Behaviours
}

// todo - consider moving to value object - not fully sure yet
public record CardReview
{
    public CardDifficulty CardDifficulty { get; init; }
    public int? RepeatCount { get; init; }
    public DateTime ReviewDate { get; init; }

    public static CardReview Create(CardDifficulty cardDifficulty, int? repeatCount = 0)
    {
        return new CardReview
        {
            CardDifficulty = cardDifficulty,
            RepeatCount = repeatCount,
            ReviewDate = DateTime.UtcNow
        };
    }
}

public record CardReviewStatus
{
    public double EaseFactor { get; init; } = 2.5;
    public int IntervalInDays { get; init; } = 0;
    public DateTime? NextReviewDate { get; init; }
    public int ReviewCount { get; init; } = 0;
    public ReviewPhase? CurrentPhase { get; init; } = ReviewPhase.Learning;
    public int? LearningStep { get; init; } = 0;
}