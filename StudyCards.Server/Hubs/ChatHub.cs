using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace StudyCards.Server.Hubs;

[Authorize]
public sealed class ChatHub : Hub<IChatClient>
{
    public override async Task OnConnectedAsync()
    {
       await Clients.All.ReceivedMessage($"{Context.ConnectionId} has joined");
    }

    public async Task SendMessage(string userEmail, string message)
    {
        await Clients.User(userEmail).ReceivedMessage(message);
    }
}

// calling from a controller example - can be a normal controller POST method
//app.MapPost("api/broadcast", async([FromBody] string message, IHubContext<ChatHub, IChatClient> context) =>
//{
//   await context.Clients.All.ReceivedMessage(message);
//   return Results.NoContent();
//});