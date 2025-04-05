using StudyCards.Domain.Enums;
using System.Text.Json.Serialization;

namespace StudyCards.Domain.Entities;

public record Card : EntityBase
{
    public Guid DeckId { get; init; }
    public string CardFront { get; init; } = string.Empty;
    public string CardBack { get; init; } = string.Empty;
    public IEnumerable<CardReview> CardReviews { get; init; } = Array.Empty<CardReview>();
}

public record CardReview
{
    public Guid CardReviewId { get; init; }
    public CardDifficulty CardDifficulty { get; init; }
    public DateTime ReviewDate { get; init; }
}
