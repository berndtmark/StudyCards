using System.ComponentModel.DataAnnotations;

namespace StudyCards.Api.Models.Request;

public class UpdateCardRequest
{
    [Required]
    public Guid CardId { get; set; }
    [Required]
    public Guid DeckId { get; set; }
    [Required]
    public string CardFront { get; set; } = string.Empty;
    [Required]
    public string CardBack { get; set; } = string.Empty;
}
