using StudyCards.Server.Models.Response;

namespace StudyCard.Server.Tests.Models;

[TestClass]
public class DeckResponseTests
{
    [TestMethod]
    public void DeckResponse_HasReviewsToday_WithReviewsToday_NotEnoughDone_ReturnsTrue()
    {
        // Arrange
        var deckResponse = new DeckResponse
        {
            DeckSettings = new DeckSettingsResponse { ReviewsPerDay = 5 },
            CardCount = 10,
            DeckReviewStatus = new DeckReviewStatusResponse
            {
                LastReview = DateTime.UtcNow,
                ReviewCount = 3
            }
        };

        // Act
        var result = deckResponse.HasReviewsToday;

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void DeckResponse_WithLessCardsThanReviews_HasReviewsToday_EnoughDone_ReturnsTrue()
    {
        // Arrange
        var deckResponse = new DeckResponse
        {
            DeckSettings = new DeckSettingsResponse { ReviewsPerDay = 5 },
            CardCount = 2,
            DeckReviewStatus = new DeckReviewStatusResponse
            {
                LastReview = DateTime.UtcNow,
                ReviewCount = 3
            }
        };

        // Act
        var result = deckResponse.HasReviewsToday;

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void DeckResponse_NoReviewsToday_ReviewsNeeded_ReturnsTrue()
    {
        // Arrange
        var deckResponse = new DeckResponse
        {
            DeckSettings = new DeckSettingsResponse { ReviewsPerDay = 5 },
            CardCount = 10,
            DeckReviewStatus = new DeckReviewStatusResponse
            {
                LastReview = DateTime.UtcNow.AddDays(-1),
                ReviewCount = 11
            }
        };

        // Act
        var result = deckResponse.HasReviewsToday;

        // Assert
        Assert.IsTrue(result);
    }
}
