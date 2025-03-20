using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Telegram.WebUI.Hubs;

public class DefaultUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection) =>
        connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
}