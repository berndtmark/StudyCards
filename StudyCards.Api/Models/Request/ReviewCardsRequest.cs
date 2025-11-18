using StudyCards.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace StudyCards.Api.Models.Request;

public class ReviewCardsRequest
{
    [Required]
    public Guid DeckId { get; set; }
    [Required]
    public IEnumerable<ReviewCardRequest> Cards { get; set; } = [];
}

public class ReviewCardRequest
{
    [Required]
    public Guid CardId { get; set; }
    [Required]
    public CardDifficulty CardDifficulty { get; set; }
    [Required]
    public int? RepeatCount { get; set; }
}
