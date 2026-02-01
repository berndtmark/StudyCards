using StudyCards.Domain.Entities;
using StudyCards.Domain.Enums;

namespace StudyCards.Domain.Study.Strategy.CardScheduleReviewStrategy;

public record struct CardSchedule(Card Card, CardDifficulty Difficulty, int RepeatCount)
{
}
