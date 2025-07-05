using StudyCards.Domain.Entities;
using StudyCards.Domain.Enums;
using StudyCards.Domain.Strategy.CardScheduleReviewStrategy.Strategies;

namespace StudyCard.Domain.Tests.Strategy.CardScheduleReviewStrategy;

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
        Assert.IsFalse(card.CardReviewStatus.IntervalInDays > 365);
    }

    [TestMethod]
    public void ScheduleNext_CardWithEqualEasyAndHardReviews_EaseRemainsUnchanged()
    {
        // Arrange
        var strategy = new AnkiScheduleStrategy();
        var card = new Card
        {
            CardReviewStatus = new CardReviewStatus
            {
                ReviewCount = 5,
                EaseFactor = 2.5,
            }
        };

        // Act
        for (int i = 0; i < 7m; i++)
        {
            card = strategy.ScheduleNext(card, CardDifficulty.Easy);
        }
        for (int i = 0; i < 7; i++)
        {
            card = strategy.ScheduleNext(card, CardDifficulty.Hard);
        }

        // Assert
        Assert.AreEqual(19, card.CardReviewStatus.ReviewCount);
        Assert.AreEqual(2.5, card.CardReviewStatus.EaseFactor);
    }
}
