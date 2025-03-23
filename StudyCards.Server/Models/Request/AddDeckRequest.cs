namespace StudyCards.Server.Models.Request;

public class AddDeckRequest
{
    public string DeckName { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public int ReviewsPerDay { get; set; }
    public int NewCardsPerDay { get; set; }
}
