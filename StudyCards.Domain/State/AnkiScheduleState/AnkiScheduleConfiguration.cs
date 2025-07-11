namespace StudyCards.Domain.State.AnkiScheduleState;

public class AnkiScheduleConfiguration
{
    public readonly double EASE_BONUS = 0.15;
    public readonly double MIN_EASE = 1.3;
    public readonly double INITIAL_EASE = 2.5;
    public readonly double GRADUATED_EASY_INTERVAL = 4;
    public readonly double MAX_INTERVAL = 365;
    public readonly double MIN_INTERVAL = 1;
    public readonly double HARD_REVIEW_MULTIPLIER = 1.2;
    public readonly double EASY_REVIEW_MULTIPLIER = 1.3;
    public readonly double LAPSE_EASE_PENALTY = 0.2;
    public readonly int MAX_REPEATS_TO_CONSIDER = 3;

    public readonly double[] LEARNING_STEPS = [1, 2];
    public readonly double[] RELEARNING_STEPS = [1, 3];
}
