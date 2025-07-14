namespace StudyCards.Api.Models.Request;

public class AddCardRequest
{
    public Guid DeckId { get; set; }
    public string CardFront { get; set; } = string.Empty;
    public string CardBack { get; set; } = string.Empty;
}
