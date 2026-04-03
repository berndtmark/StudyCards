using StudyCards.Domain.Enums;

namespace StudyCards.Domain.ValueObjects;

public record CardReview
{
    public Guid CardReviewId { get; init; }
    public CardDifficulty CardDifficulty { get; init; }
    public int? RepeatCount { get; init; }
    public DateTime ReviewDate { get; init; }

    public static CardReview Create(CardDifficulty cardDifficulty, int? repeatCount = 0)
    {
        return new CardReview
        {
            CardReviewId = Guid.NewGuid(),
            CardDifficulty = cardDifficulty,
            RepeatCount = repeatCount,
            ReviewDate = DateTime.UtcNow
        };
    }
}