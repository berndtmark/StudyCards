using StudyCards.Domain.Entities;

namespace StudyCards.Server.Models.Response;

public class DeckResponse
{
    public Guid Id { get; init; }
    public string DeckName { get; init; } = string.Empty;
    public string? Description { get; init; } = string.Empty;
    public string UserEmail { get; init; } = string.Empty;
    public DeckSettings DeckSettings { get; init; } = new();
    public DeckReviewStatus DeckReviewStatus { get; init; } = new();
    public DateTime CreatedDate { get; init; } = DateTime.UtcNow;
    public DateTime UpdatedDate { get; init; } = DateTime.UtcNow;
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
