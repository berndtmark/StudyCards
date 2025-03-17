
using System.Text.Json.Serialization;

namespace StudyCards.Domain.Entities;

public record Deck : EntityBase
{
    public string DeckName { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string UserEmail { get; init; } = string.Empty;
    public DeckSettings DeckSettings { get; init; } = new();

    [JsonIgnore]
    public override object PartitionKey => UserEmail;
}

public record DeckSettings
{
    public int ReviewsPerDay { get; init; }
    public int NewCardsPerDay { get; init; }
}
