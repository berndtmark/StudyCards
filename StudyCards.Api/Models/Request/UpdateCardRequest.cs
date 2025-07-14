namespace StudyCards.Server.Models.Request;

public class UpdateCardRequest
{
    public Guid CardId { get; set; }
    public Guid DeckId { get; set; }
    public string CardFront { get; set; } = string.Empty;
    public string CardBack { get; set; } = string.Empty;
}
