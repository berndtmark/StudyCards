using System.ComponentModel.DataAnnotations;

namespace StudyCards.Api.Models.Request;

public class AddCardRequest
{
    [Required]
    public Guid DeckId { get; set; }
    [Required]
    public string CardFront { get; set; } = string.Empty;
    [Required]
    public string CardBack { get; set; } = string.Empty;
}
