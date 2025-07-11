using StudyCards.Domain.Entities;
using StudyCards.Domain.Enums;
using StudyCards.Domain.State.AnkiState;
using StudyCards.Domain.State.AnkiState.States;

namespace StudyCard.Domain.Tests.State;

[TestClass]
public class LearningStateTests
{
    private static readonly AnkiScheduleConfiguration DefaultConfig = new()
    {
        LEARNING_STEPS = [1.0, 10.0],
        MIN_INTERVAL = 1.0,
        GRADUATED_EASY_INTERVAL = 4.0,
        INITIAL_EASE = 2.5
    };

    private static CardReviewStatus CreateInitialStatus() => new()
    {
        LearningStep = 0,
        IntervalInDays = 1,
        NextReviewDate = DateTime.UtcNow,
        EaseFactor = 2.5
    };

    [TestMethod]
    public void Schedule_WhenHardDifficulty_ReturnsToFirstStep()
    {
        // Arrange
        var state = new LearningState();
        var status = CreateInitialStatus() with { LearningStep = 1 };

        // Act
        var (newStatus, phase) = state.Schedule(status, CardDifficulty.Hard, 0, DefaultConfig);

        // Assert
        Assert.AreEqual(0, newStatus.LearningStep);
        Assert.AreEqual(DefaultConfig.LEARNING_STEPS[0], newStatus.IntervalInDays);
        Assert.AreEqual(ReviewPhase.Learning, phase);
    }

    [TestMethod]
    public void Schedule_WhenRepeatedFailure_ReturnsToFirstStep()
    {
        // Arrange
        var state = new LearningState();
        var status = CreateInitialStatus() with { LearningStep = 1 };

        // Act
        var (newStatus, phase) = state.Schedule(status, CardDifficulty.Medium, 2, DefaultConfig);

        // Assert
        Assert.AreEqual(0, newStatus.LearningStep);
        Assert.AreEqual(DefaultConfig.LEARNING_STEPS[0], newStatus.IntervalInDays);
        Assert.AreEqual(ReviewPhase.Learning, phase);
    }

    [TestMethod]
    public void Schedule_WhenCompletingAllSteps_GraduatesToReview()
    {
        // Arrange
        var state = new LearningState();
        var status = CreateInitialStatus() with { LearningStep = 1 };

        // Act
        var (newStatus, phase) = state.Schedule(status, CardDifficulty.Medium, 0, DefaultConfig);

        // Assert
        Assert.IsNull(newStatus.LearningStep);
        Assert.AreEqual(DefaultConfig.MIN_INTERVAL, newStatus.IntervalInDays); // MIN_INTERVAL
        Assert.AreEqual(DefaultConfig.INITIAL_EASE, newStatus.EaseFactor); // INITIAL_EASE
        Assert.AreEqual(ReviewPhase.Reviewing, phase);
    }

    [TestMethod]
    public void Schedule_WhenCompletingAllStepsWithEasy_GraduatesWithLongerInterval()
    {
        // Arrange
        var state = new LearningState();
        var status = CreateInitialStatus() with { LearningStep = 1 };

        // Act
        var (newStatus, phase) = state.Schedule(status, CardDifficulty.Easy, 0, DefaultConfig);

        // Assert
        Assert.IsNull(newStatus.LearningStep);
        Assert.AreEqual(DefaultConfig.GRADUATED_EASY_INTERVAL, newStatus.IntervalInDays); // GRADUATED_EASY_INTERVAL
        Assert.AreEqual(DefaultConfig.INITIAL_EASE, newStatus.EaseFactor); // INITIAL_EASE
        Assert.AreEqual(ReviewPhase.Reviewing, phase);
    }

    [TestMethod]
    public void Schedule_WhenInMiddleOfLearning_MovesToNextStep()
    {
        // Arrange
        var state = new LearningState();
        var status = CreateInitialStatus();

        // Act
        var (newStatus, phase) = state.Schedule(status, CardDifficulty.Medium, 0, DefaultConfig);

        // Assert
        Assert.AreEqual(1, newStatus.LearningStep);
        Assert.AreEqual(DefaultConfig.LEARNING_STEPS[1], newStatus.IntervalInDays);
        Assert.AreEqual(ReviewPhase.Learning, phase);
    }
}