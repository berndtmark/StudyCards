namespace StudyCards.Domain.Study.State.AnkiState;

public class AnkiScheduleConfiguration
{
    public double EASE_BONUS = 0.15;
    public double MIN_EASE = 1.3;
    public double INITIAL_EASE = 2.5;
    public double GRADUATED_EASY_INTERVAL = 4;
    public double MAX_INTERVAL = 365;
    public double MIN_INTERVAL = 1;
    public double HARD_REVIEW_MULTIPLIER = 1.2;
    public double EASY_REVIEW_MULTIPLIER = 1.3;
    public double LAPSE_EASE_PENALTY = 0.2;
    public int MAX_REPEATS_TO_CONSIDER = 3;
    public double RELEARNING_GRADUATED_INTERVAL = 2.0;
    public double FORGET_REPEAT_THRESHOLD = 3;

    public double[] LEARNING_STEPS = [1, 2];
    public double[] RELEARNING_STEPS = [1, 3];
}
