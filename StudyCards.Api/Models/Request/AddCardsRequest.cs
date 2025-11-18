using System.ComponentModel.DataAnnotations;

namespace StudyCards.Api.Models.Request;

public class AddCardsRequest
{
    [Required]
    public Guid DeckId { get; set; }
    [Required]
    public IList<CardText> Cards { get; set; } = [];
}

public record CardText([Required] string CardFront, [Required] string CardBack);
