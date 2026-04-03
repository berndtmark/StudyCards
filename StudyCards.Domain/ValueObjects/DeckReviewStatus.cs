namespace StudyCards.Domain.ValueObjects;

public record DeckReviewStatus
{
    public DateTime LastReview { get; init; }
    public int ReviewCount { get; init; }
}