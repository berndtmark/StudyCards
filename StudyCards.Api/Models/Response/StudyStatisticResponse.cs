namespace StudyCards.Api.Models.Response;

public class StudyStatisticResponse
{
    public DateTime DateRecorded { get; set; }
    public string Name { get; set; } = string.Empty;
    public int CardsStudied { get; set; }
}
