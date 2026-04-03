using StudyCards.Domain.Enums;

namespace StudyCards.Domain.ValueObjects;

public record CardReviewStatus
{
    public double EaseFactor { get; init; } = 2.5;
    public int IntervalInDays { get; init; } = 0;
    public DateTime? NextReviewDate { get; init; }
    public int ReviewCount { get; init; } = 0;
    public ReviewPhase? CurrentPhase { get; init; } = ReviewPhase.Learning;
    public int? LearningStep { get; init; } = 0;
}
