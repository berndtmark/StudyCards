using StudyCards.Domain.Enums;

namespace StudyCards.Domain.Entities;

public record Card
{
    public Guid CardId { get; set; }
    public string UserEmail { get; set; } = string.Empty;
    public string CardFront { get; set; } = string.Empty;
    public string CardBack { get; set; } = string.Empty;
    public IEnumerable<CardReview> CardReviews { get; set; } = Array.Empty<CardReview>();
}

public record CardReview
{
    public Guid CardReviewId { get; set; }
    public CardDifficulty CardDifficulty { get; set; }
    public DateTime ReviewDate { get; set; }
}
