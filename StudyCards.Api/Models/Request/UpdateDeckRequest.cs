using StudyCards.Api.Models.Validators;
using System.ComponentModel.DataAnnotations;

namespace StudyCards.Api.Models.Request;

[MaxNewCardsValidator]
public class UpdateDeckRequest : IAddUpdateDeckRequest
{
    [Required]
    public Guid DeckId { get; set; }
    [Required]
    public string DeckName { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    [Required]
    public int ReviewsPerDay { get; set; }
    [Required]
    public int NewCardsPerDay { get; set; }
}
