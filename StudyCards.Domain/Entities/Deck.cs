using StudyCards.Domain.DomainEvents;
using StudyCards.Domain.Extensions;
using StudyCards.Domain.ValueObjects;

namespace StudyCards.Domain.Entities;

public record Deck : EntityBase
{
    public Guid UserId { get; internal init; }
    public string DeckName { get; internal init; } = string.Empty;
    public string? Description { get; internal init; } = string.Empty;
    public int? CardCount { get; internal init; }
    public DeckSettings DeckSettings { get; internal init; } = new();
    public DeckReviewStatus DeckReviewStatus { get; internal init; } = new();

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

    #region Behaviours
    public static Deck Create(Guid userId, string deckName, string? description, DeckSettings deckSettings)
    {
        return new Deck
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            DeckName = deckName,
            Description = description,
            DeckSettings = deckSettings,
            CardCount = 0
        };
    }

    public Deck Update(string deckName, string? description, DeckSettings deckSettings)
    {
        return this with
        {
            DeckName = deckName,
            Description = description,
            DeckSettings = deckSettings
        };
    }

    public Deck StudyCompleted(int cardReviewCount)
    {
        var isFirstReviewToday = !DeckReviewStatus.LastReview.Date.IsSameDay();

        var result = this with
        {
            DeckReviewStatus = new DeckReviewStatus
            {
                LastReview = DateTime.UtcNow,
                ReviewCount = isFirstReviewToday ? cardReviewCount : DeckReviewStatus.ReviewCount + cardReviewCount,
            }
        };

        result.Raise(new StudyCompletedDomainEvent(result.Id, result.UserId, result.DeckName, cardReviewCount));
        return result;
    }

    public Deck UpdateCardCount(int cardCount)
    {
        return this with
        {
            CardCount = cardCount
        };
    }
    #endregion Behaviours
}
