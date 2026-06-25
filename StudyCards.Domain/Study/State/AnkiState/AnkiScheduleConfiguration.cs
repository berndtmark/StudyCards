namespace StudyCards.Domain.Study.State.AnkiState;

public record AnkiScheduleConfiguration
{
    public double EASE_BONUS { get; init; } = 0.15;
    public double MIN_EASE { get; init; } = 1.3;
    public double INITIAL_EASE { get; init; } = 2.5;
    public double GRADUATED_EASY_INTERVAL { get; init; } = 4;
    public double MAX_INTERVAL { get; init; } = 365;
    public double MIN_INTERVAL { get; init; } = 1;
    public double HARD_REVIEW_MULTIPLIER { get; init; } = 1.2;
    public double EASY_REVIEW_MULTIPLIER { get; init; } = 1.3;
    public double LAPSE_EASE_PENALTY { get; init; } = 0.2;
    public int MAX_REPEATS_TO_CONSIDER { get; init; } = 3;
    public double RELEARNING_GRADUATED_INTERVAL { get; init; } = 2.0;
    public double FORGET_REPEAT_THRESHOLD { get; init; } = 3;

    public double[] LEARNING_STEPS { get; init; } = [1, 2];
    public double[] RELEARNING_STEPS { get; init; } = [1, 3];
}
