using StudyCards.Domain.Enums;

namespace StudyCards.Api.Models.Request;

public class ReviewCardsRequest
{
    public Guid DeckId { get; set; }
    public IEnumerable<ReviewCardRequest> Cards { get; set; } = [];
}

public class ReviewCardRequest
{
    public Guid CardId { get; set; }
    public CardDifficulty CardDifficulty { get; set; }
    public int? RepeatCount { get; set; }
}
