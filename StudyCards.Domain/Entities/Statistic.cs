namespace StudyCards.Domain.Entities;

public abstract record class Statistic : EntityBase
{
    public Guid UserId { get; internal init; }
    public Guid DeckId { get; internal init; }
    public DateTime DateRecorded { get; internal init; } = DateTime.UtcNow;
}

public record class StudyStatistic : Statistic
{
    public string Name { get; internal init; } = string.Empty;
    public int CardsStudied { get; internal init; }

    #region Behaviours
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
    #endregion Behaviours
}
