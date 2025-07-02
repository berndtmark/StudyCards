namespace StudyCards.Server.Models.Response;

public class AddCardsResponse
{
    public IEnumerable<CardResponse> CardsAdded { get; set; } = [];
    public IEnumerable<CardResponse> CardsSkipped { get; set; } = [];
}
