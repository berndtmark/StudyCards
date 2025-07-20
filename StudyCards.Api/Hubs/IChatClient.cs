namespace StudyCards.Api.Hubs;

public interface IChatClient
{
    Task ReceivedMessage(string message);
}
