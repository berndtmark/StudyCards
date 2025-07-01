using StudyCards.Domain.Entities;

namespace StudyCard.Domain.Tests.Entities;

[TestClass]
public class DeckTests
{
    [TestMethod]
    public void DeckResponse_HasReviewsToday_WithReviewsToday_NotEnoughDone()
    {
        // Arrange
        var deckResponse = new Deck
        {
            DeckSettings = new DeckSettings { ReviewsPerDay = 5 },
            CardCount = 10,
            DeckReviewStatus = new DeckReviewStatus
            {
                LastReview = DateTime.UtcNow,
                ReviewCount = 3
            }
        };

        // Act
        var result = deckResponse.CardNoToReview;

        // Assert
        Assert.AreEqual(2, result);
    }

    [TestMethod]
    public void DeckResponse_WithLessCardsThanReviews_HasReviewsToday_EnoughDone()
    {
        // Arrange
        var deckResponse = new Deck
        {
            DeckSettings = new DeckSettings { ReviewsPerDay = 5 },
            CardCount = 2,
            DeckReviewStatus = new DeckReviewStatus
            {
                LastReview = DateTime.UtcNow,
                ReviewCount = 3
            }
        };

        // Act
        var result = deckResponse.CardNoToReview;

        // Assert
        Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void DeckResponse_NoReviewsToday_ReviewsNeeded()
    {
        // Arrange
        var deckResponse = new Deck
        {
            DeckSettings = new DeckSettings { ReviewsPerDay = 5 },
            CardCount = 10,
            DeckReviewStatus = new DeckReviewStatus
            {
                LastReview = DateTime.UtcNow.AddDays(-1),
                ReviewCount = 11
            }
        };

        // Act
        var result = deckResponse.CardNoToReview;

        // Assert
        Assert.AreEqual(5, result);
    }

    [TestMethod]
    public void DeckResponse_NoReviewsToday_LessCardsThanReviews_ReviewsNeeded()
    {
        // Arrange
        var deckResponse = new Deck
        {
            DeckSettings = new DeckSettings { ReviewsPerDay = 5 },
            CardCount = 2,
            DeckReviewStatus = new DeckReviewStatus
            {
                LastReview = DateTime.UtcNow.AddDays(-1),
                ReviewCount = 11
            }
        };

        // Act
        var result = deckResponse.CardNoToReview;

        // Assert
        Assert.AreEqual(2, result);
    }
}
