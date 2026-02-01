using StudyCards.Domain.Entities;
using StudyCards.Domain.Enums;
using StudyCards.Domain.Study.Strategy.CardScheduleReviewStrategy.Strategies;

namespace StudyCards.Domain.Tests.Study.Strategy.CardScheduleReviewStrategy;

[TestClass]
public class AnkiScheduleStrategyTests
{
    [TestMethod]
    public void ScheduleNext_CardWithFirstHardReview_IntervalSetToTomorrow()
    {
        // Arrange
        var strategy = new AnkiScheduleStrategy();
        var card = new Card
        {
            CardReviewStatus = new CardReviewStatus
            {
                ReviewCount = 0
            }
        };

        // Act
        var result = strategy.ScheduleNext(card, CardDifficulty.Hard);

        // Assert
        Assert.AreEqual(1.0, result.CardReviewStatus.IntervalInDays);
        Assert.AreEqual(DateTime.UtcNow.AddDays(1).Date, result.CardReviewStatus.NextReviewDate!.Value.Date);
    }

    [TestMethod]
    public void ScheduleNext_CardWithManyEasyReviews_IntervalDoesNotExceedMax()
    {
        // Arrange
        var strategy = new AnkiScheduleStrategy();
        var card = new Card
        {
            CardReviewStatus = new CardReviewStatus
            {
                ReviewCount = 5,
            }
        };

        // Act
        for (int i = 0; i < 15; i++)
        {
            card = strategy.ScheduleNext(card, CardDifficulty.Easy);
        }

        // Assert
        Assert.AreEqual(20, card.CardReviewStatus.ReviewCount);
        Assert.IsLessThanOrEqualTo(365, card.CardReviewStatus.IntervalInDays);
    }
}
