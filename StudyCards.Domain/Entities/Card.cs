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

        Raise(new CardReviewedDomainEvent(Id, CardReviewStatus.CurrentPhase, result.CardReviewStatus));
        return result;
    }
}

public record CardReview
{
    public Guid CardReviewId { get; init; }
    public CardDifficulty CardDifficulty { get; init; }
    public int? RepeatCount { get; init; }
    public DateTime ReviewDate { get; init; }
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