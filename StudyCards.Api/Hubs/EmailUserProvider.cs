using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace StudyCards.Api.Hubs;

public class EmailUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        return connection.User?.FindFirst(ClaimTypes.Email)?.Value;
    }
}