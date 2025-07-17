using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace StudyCards.Api.Hubs;

// 💡 External Usage Example:
// public class NotificationService(IHubContext<ChatHub, IChatClient> hubContext)
// {
//     public async Task BroadcastMessage(string message)
//     {
//         await hubContext.Clients.All.ReceivedMessage(message);
//     }
// }

[Authorize]
public sealed class ChatHub : Hub<IChatClient>
{
    private readonly ILogger<ChatHub> _logger;

    // SignalR does not work with primary constructors
    public ChatHub(ILogger<ChatHub> logger)
    {
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation($"{Context.ConnectionId} has joined the hub");
        await base.OnConnectedAsync();
    }

    public async Task SendMessage(string userEmail, string message)
    {
        await Clients.User(userEmail).ReceivedMessage(message);
    }
}
