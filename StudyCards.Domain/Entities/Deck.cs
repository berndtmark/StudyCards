using StudyCards.Domain.DomainEvents;
using StudyCards.Domain.Extensions;

namespace StudyCards.Domain.Entities;

public record Deck : EntityBase
{
    public string DeckName { get; init; } = string.Empty;
    public string? Description { get; init; } = string.Empty;
    public string UserEmail { get; init; } = string.Empty;
    public int? CardCount { get; init; }
    public DeckSettings DeckSettings { get; init; } = new();
    public DeckReviewStatus DeckReviewStatus { get; init; } = new();

    public int CardNoToReview
    {
        get
        {
            var maxReviews = Math.Min(DeckSettings.ReviewsPerDay, CardCount ?? DeckSettings.ReviewsPerDay);

            return DeckReviewStatus.LastReview.IsSameDay() ?
                    Math.Max(maxReviews - DeckReviewStatus.ReviewCount, 0) :
                    maxReviews;

        }
    }

    public Deck StudyCompleted(int cardReviewCount)
    {
        var isFirstReviewToday = DeckReviewStatus.LastReview.Date.IsSameDay();

        var result = this with
        {
            DeckReviewStatus = new DeckReviewStatus
            {
                LastReview = DateTime.UtcNow,
                ReviewCount = isFirstReviewToday ? cardReviewCount : DeckReviewStatus.ReviewCount + cardReviewCount,
            }
        };

        result.Raise(new StudyCompletedDomainEvent(result.Id, result.DeckName, cardReviewCount));
        return result;
    }
}

public record DeckSettings
{
    public int ReviewsPerDay { get; init; }
    public int NewCardsPerDay { get; init; }
}

public record DeckReviewStatus
{
    public DateTime LastReview { get; init; }
    public int ReviewCount { get; init; }
}
