using StudyCards.Domain.Entities;
using StudyCards.Domain.Enums;
using StudyCards.Domain.State.AnkiState;
using StudyCards.Domain.State.AnkiState.States;

namespace StudyCard.Domain.Tests.State;

[TestClass]
public class RelearningStateTests
{
    private static readonly AnkiScheduleConfiguration DefaultConfig = new()
    {
        RELEARNING_STEPS = [1.0, 5.0],
        MIN_EASE = 1.3,
        LAPSE_EASE_PENALTY = 0.2,
        MAX_REPEATS_TO_CONSIDER = 2,
        RELEARNING_GRADUATED_INTERVAL = 7.0
    };

    private static CardReviewStatus CreateInitialStatus() => new()
    {
        LearningStep = 0,
        IntervalInDays = 1,
        NextReviewDate = DateTime.UtcNow,
        EaseFactor = 2.5
    };

    [TestMethod]
    public void Schedule_WhenHardAtFirstStep_AppliesEasePenalty()
    {
        // Arrange
        var state = new RelearningState();
        var status = CreateInitialStatus();

        // Act
        var (newStatus, phase) = state.Schedule(status, [], CardDifficulty.Hard, 0, DefaultConfig);

        // Assert
        Assert.AreEqual(0, newStatus.LearningStep);
        Assert.AreEqual(1, newStatus.IntervalInDays);
        Assert.AreEqual(2.3, newStatus.EaseFactor); // 2.5 - 0.2
        Assert.AreEqual(ReviewPhase.Relearning, phase);
    }

    [TestMethod]
    public void Schedule_WhenRepeatedFailureAtFirstStep_AppliesMultipleEasePenalties()
    {
        // Arrange
        var state = new RelearningState();
        var status = CreateInitialStatus();

        // Act
        var (newStatus, phase) = state.Schedule(status, [], CardDifficulty.Hard, 2, DefaultConfig);

        // Assert
        Assert.AreEqual(0, newStatus.LearningStep);
        Assert.AreEqual(1, newStatus.IntervalInDays);
        Assert.AreEqual(2.1, Math.Round(newStatus.EaseFactor, 1)); // 2.5 - 0.2 - 0.2
        Assert.AreEqual(ReviewPhase.Relearning, phase);
    }

    [TestMethod]
    public void Schedule_WhenSuccessfulAtFirstStep_MovesToNextStep()
    {
        // Arrange
        var state = new RelearningState();
        var status = CreateInitialStatus();

        // Act
        var (newStatus, phase) = state.Schedule(status, [], CardDifficulty.Medium, 0, DefaultConfig);

        // Assert
        Assert.AreEqual(1, newStatus.LearningStep);
        Assert.AreEqual(5, newStatus.IntervalInDays);
        Assert.AreEqual(ReviewPhase.Relearning, phase);
    }

    [TestMethod]
    public void Schedule_WhenCompletingAllSteps_GraduatesToReview()
    {
        // Arrange
        var state = new RelearningState();
        var status = CreateInitialStatus() with { LearningStep = 1 };

        // Act
        var (newStatus, phase) = state.Schedule(status, [], CardDifficulty.Medium, 0, DefaultConfig);

        // Assert
        Assert.IsNull(newStatus.LearningStep);
        Assert.AreEqual(7, newStatus.IntervalInDays); // RELEARNING_GRADUATED_INTERVAL
        Assert.AreEqual(ReviewPhase.Reviewing, phase);
    }

    [TestMethod]
    public void Schedule_WhenEaseFactorWouldGoTooLow_ClampsToMinEase()
    {
        // Arrange
        var state = new RelearningState();
        var status = CreateInitialStatus() with { EaseFactor = 1.4 };

        // Act
        var (newStatus, phase) = state.Schedule(status, [], CardDifficulty.Hard, 2, DefaultConfig);

        // Assert
        Assert.AreEqual(1.3, newStatus.EaseFactor); // MIN_EASE
        Assert.AreEqual(ReviewPhase.Relearning, phase);
    }
}