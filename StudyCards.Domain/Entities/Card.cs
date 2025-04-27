using StudyCards.Domain.Enums;

namespace StudyCards.Domain.Entities;

public record Card : EntityBase
{
    public Guid DeckId { get; init; }
    public string CardFront { get; init; } = string.Empty;
    public string CardBack { get; init; } = string.Empty;
    public ICollection<CardReview> CardReviews { get; init; } = [];
}

public record CardReview
{
    public Guid CardReviewId { get; init; }
    public CardDifficulty CardDifficulty { get; init; }
    public int? RepeatCount { get; init; }
    public DateTime ReviewDate { get; init; }
}
