namespace StudyCards.Data.Entities;

public class Card
{
    public Guid CardId { get; set; }
    public string UserEmail { get; set; } = string.Empty;
    public string CardFront { get; set; } = string.Empty;
    public string CardBack { get; set; } = string.Empty;
}
