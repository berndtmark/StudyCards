namespace StudyCards.Server.Models.Response;

public class CardResponseWithReviews
{
    public Guid Id { get; init; }
    public Guid DeckId { get; init; }
    public string CardFront { get; init; } = string.Empty;
    public string CardBack { get; init; } = string.Empty;
    public DateTime CreatedDate { get; init; } = DateTime.UtcNow;
    public DateTime UpdatedDate { get; init; } = DateTime.UtcNow;
    public IList<CardReviewResponse> CardReviews { get; init; } = [];
}

public class CardReviewResponse
{
    public Guid CardReviewId { get; init; }
    public string CardDifficulty { get; init; } = string.Empty;
    public DateTime ReviewDate { get; init; } = DateTime.UtcNow;
}