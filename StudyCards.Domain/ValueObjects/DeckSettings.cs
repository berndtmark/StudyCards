namespace StudyCards.Domain.ValueObjects;

public record DeckSettings
{
    public int ReviewsPerDay { get; init; }
    public int NewCardsPerDay { get; init; }
}
