namespace StudyCards.Domain.Entities;

public record Deck
{
    public Guid DeckId { get; init; }
    public string DeckName { get; init; } = string.Empty;
    public string UserEmail { get; init; } = string.Empty;
    public DeckSettings DeckSettings { get; init; } = new();
}

public record DeckSettings
{
    public int ReviewsPerDay { get; init; }
    public int NewCardsPerDay { get; init; }
}