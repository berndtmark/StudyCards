using StudyCards.Application.Helpers;
using StudyCards.Domain.Entities;

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

    public bool HasReviewsToday
    {
        get
        {
            var maxReviews = Math.Min(DeckSettings.ReviewsPerDay, CardCount ?? 0);
            if (maxReviews == 0) return false; // No cards to review

            if (!DeckReviewStatus.LastReview.Date.IsSameDay())
                return true; // New day and cards exist

            return DeckReviewStatus.ReviewCount < maxReviews;
        }
    }
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
