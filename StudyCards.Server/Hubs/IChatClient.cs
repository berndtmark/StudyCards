namespace StudyCards.Server.Hubs;

public interface IChatClient
{
    Task ReceivedMessage(string message);
}
