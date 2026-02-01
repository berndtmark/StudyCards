using StudyCards.Domain.Entities;
using StudyCards.Domain.Enums;
using StudyCards.Domain.Study.State.AnkiState.States;
using StudyCards.Domain.Study.State.AnkiState;

namespace StudyCards.Domain.Tests.Study.State;

[TestClass]
public class ReviewingStateTests
{
    private static readonly AnkiScheduleConfiguration DefaultConfig = new()
    {
        MIN_INTERVAL = 1.0,
        MAX_INTERVAL = 365.0,
        MIN_EASE = 1.3,
        EASE_BONUS = 0.15,
        HARD_REVIEW_MULTIPLIER = 1.2,
        EASY_REVIEW_MULTIPLIER = 1.3,
        RELEARNING_STEPS = [1.0, 5.0],
        FORGET_REPEAT_THRESHOLD = 3,
    };

    private static CardReviewStatus CreateInitialStatus() => new()
    {
        LearningStep = null,
        IntervalInDays = 10,
        NextReviewDate = DateTime.UtcNow,
        EaseFactor = 2.5
    };

    [TestMethod]
    public void Schedule_WhenConsecutiveHardRatings_MovesToRelearning()
    {
        // Arrange
        var state = new ReviewingState();
        var status = CreateInitialStatus();
        var pastReviews = new[] 
        {
            new CardReview { CardDifficulty = CardDifficulty.Hard }
        };

        // Act
        var (newStatus, phase) = state.Schedule(status, pastReviews, CardDifficulty.Hard, 0, DefaultConfig);

        // Assert
        Assert.AreEqual(0, newStatus.LearningStep);
        Assert.AreEqual(1, newStatus.IntervalInDays);
        Assert.AreEqual(ReviewPhase.Relearning, phase);
    }

    [TestMethod]
    public void Schedule_WhenRepeatedFailure_MovesToRelearning()
    {
        // Arrange
        var state = new ReviewingState();
        var status = CreateInitialStatus();

        // Act
        var (newStatus, phase) = state.Schedule(status, [], CardDifficulty.Medium, (int)DefaultConfig.FORGET_REPEAT_THRESHOLD + 1, DefaultConfig);

        // Assert
        Assert.AreEqual(0, newStatus.LearningStep);
        Assert.AreEqual(1, newStatus.IntervalInDays);
        Assert.AreEqual(ReviewPhase.Relearning, phase);
    }

    [TestMethod]
    public void Schedule_WhenEasy_IncreasesEaseAndInterval()
    {
        // Arrange
        var state = new ReviewingState();
        var status = CreateInitialStatus();

        // Act
        var (newStatus, phase) = state.Schedule(status, [], CardDifficulty.Easy, 0, DefaultConfig);

        // Assert
        Assert.AreEqual(2.65, newStatus.EaseFactor); // 2.5 + 0.15
        Assert.AreEqual(34, newStatus.IntervalInDays); // 10 * 2.65 * 1.3 (rounded)
        Assert.AreEqual(ReviewPhase.Reviewing, phase);
    }

    [TestMethod]
    public void Schedule_WhenMedium_MaintainsEaseAndIncreasesInterval()
    {
        // Arrange
        var state = new ReviewingState();
        var status = CreateInitialStatus();

        // Act
        var (newStatus, phase) = state.Schedule(status, [], CardDifficulty.Medium, 0, DefaultConfig);

        // Assert
        Assert.AreEqual(2.5, newStatus.EaseFactor);
        Assert.AreEqual(25, newStatus.IntervalInDays); // 10 * 2.5 (rounded)
        Assert.AreEqual(ReviewPhase.Reviewing, phase);
    }

    [TestMethod]
    public void Schedule_WhenIntervalWouldExceedMax_ClampsToMaxInterval()
    {
        // Arrange
        var state = new ReviewingState();
        var status = CreateInitialStatus() with { IntervalInDays = 300 };

        // Act
        var (newStatus, phase) = state.Schedule(status, [], CardDifficulty.Easy, 0, DefaultConfig);

        // Assert
        Assert.AreEqual(365, newStatus.IntervalInDays); // MAX_INTERVAL
        Assert.AreEqual(ReviewPhase.Reviewing, phase);
    }

    [TestMethod]
    public void Schedule_WhenEaseWouldGoTooLow_ClampsToMinEase()
    {
        // Arrange
        var state = new ReviewingState();
        var status = CreateInitialStatus() with { EaseFactor = 1.4 };

        // Act
        var (newStatus, phase) = state.Schedule(status, [], CardDifficulty.Hard, 0, DefaultConfig);

        // Assert
        Assert.AreEqual(1.3, newStatus.EaseFactor); // MIN_EASE
    }

    [TestMethod]
    public void Schedule_WhenRepeatedAndHardRating_MovesToRelearning()
    {
        // Arrange
        var state = new ReviewingState();
        var status = CreateInitialStatus();

        // Act
        var (newStatus, phase) = state.Schedule(status, [], CardDifficulty.Hard, 1, DefaultConfig);

        // Assert
        Assert.AreEqual(0, newStatus.LearningStep);
        Assert.AreEqual(1, newStatus.IntervalInDays);
        Assert.AreEqual(ReviewPhase.Relearning, phase);
    }

    [TestMethod]
    public void Schedule_WhenTooManyRepeatsAndNotEasy_MovesToRelearning()
    {
        // Arrange
        var state = new ReviewingState();
        var status = CreateInitialStatus();

        // Act - Using Medium difficulty with repeat count at threshold
        var (newStatus, phase) = state.Schedule(status, [], CardDifficulty.Medium, (int)DefaultConfig.FORGET_REPEAT_THRESHOLD, DefaultConfig);

        // Assert
        Assert.AreEqual(0, newStatus.LearningStep);
        Assert.AreEqual(1, newStatus.IntervalInDays);
        Assert.AreEqual(ReviewPhase.Relearning, phase);
    }

    [TestMethod]
    public void Schedule_WhenTooManyRepeatsButEasy_StaysInReviewing()
    {
        // Arrange
        var state = new ReviewingState();
        var status = CreateInitialStatus();

        // Act - Using Easy difficulty even with high repeat count
        var (newStatus, phase) = state.Schedule(status, [], CardDifficulty.Easy, (int)DefaultConfig.FORGET_REPEAT_THRESHOLD, DefaultConfig);

        // Assert
        Assert.AreEqual(ReviewPhase.Reviewing, phase);
        Assert.IsNull(newStatus.LearningStep);
        Assert.AreEqual(2.65, newStatus.EaseFactor); // 2.5 + 0.15
    }
}