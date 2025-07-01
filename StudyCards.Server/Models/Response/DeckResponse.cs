namespace StudyCards.Server.Models.Response;

public class DeckResponse
{
    public Guid Id { get; init; }
    public string DeckName { get; init; } = string.Empty;
    public string? Description { get; init; } = string.Empty;
    public string UserEmail { get; init; } = string.Empty;
    public int? CardCount { get; init; }
    public DeckSettingsResponse DeckSettings { get; init; } = new();
    public DeckReviewStatusResponse DeckReviewStatus { get; init; } = new();
    public DateTime CreatedDate { get; init; } = DateTime.UtcNow;
    public DateTime UpdatedDate { get; init; } = DateTime.UtcNow;
    public int CardNoToReview { get; init; }

    public bool HasReviewsToday => CardNoToReview > 0;
}

public record DeckSettingsResponse
{
    public int ReviewsPerDay { get; init; }
    public int NewCardsPerDay { get; init; }
}

public record DeckReviewStatusResponse
{
    public DateTime LastReview { get; init; }
    public int ReviewCount { get; init; }
}
