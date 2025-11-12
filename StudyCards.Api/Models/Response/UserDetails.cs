namespace StudyCards.Api.Models.Response;

public class UserDetails
{
    public string UserEmail { get; set; } = string.Empty;
    public DateTime LastLogin { get; set; }
    public int LoginCount { get; set; }
    public DateTime UserCreated { get; set; }
}
