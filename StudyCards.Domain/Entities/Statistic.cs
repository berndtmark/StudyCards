namespace StudyCards.Domain.Entities;

public abstract record class Statistic : EntityBase
{
    public Guid UserId { get; init; }
    public Guid DeckId { get; init; }
    public DateTime DateRecorded { get; init; } = DateTime.UtcNow;
}

public record class StudyStatistic : Statistic
{
    public string Name { get; init; } = string.Empty;
    public int CardsStudied { get; init; }

    public static StudyStatistic Create(Guid userId, Guid deckId, string name, int cardsStudied)
    {
        return new StudyStatistic
        {
            UserId = userId,
            DeckId = deckId,
            Name = name,
            CardsStudied = cardsStudied,
        };
    }

    public StudyStatistic Update(int cardsStudied)
    {
        return this with
        {
            CardsStudied = CardsStudied + cardsStudied,
        };
    }
}
