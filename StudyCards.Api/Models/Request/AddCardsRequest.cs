namespace StudyCards.Api.Models.Request;

public class AddCardsRequest
{
    public Guid DeckId { get; set; }
    public IList<CardText> Cards { get; set; } = [];
}

public record CardText(string CardFront, string CardBack);
