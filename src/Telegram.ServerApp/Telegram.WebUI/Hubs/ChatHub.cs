using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Telegram.Application.Services;
using Telegram.Domain.Entities;
using Telegram.WebUI.Models.Messages;

namespace Telegram.WebUI.Hubs;

[Authorize]
public class ChatHub(IUserService userService, IMessageService messageService, IChatRoomService chatRoomService, IMapper mapper) : Hub
{
    public async Task SendMessage(string user, string body)
    {
        var senderId = GetRequiredUserId();
        var receiverId = Convert.ToInt64(user);
        var receiver = await userService.GetByIdAsync(receiverId);
        ChatRoom chatRoom;
        try
        {
            chatRoom = await chatRoomService.GetByUsersIdAsync(senderId, receiverId);
        }
        catch
        {
            chatRoom = await chatRoomService.CreateAsync(new ChatRoom { FirstUserId = senderId, SecondUserId = receiverId });
        }
        var created = new Message { SenderId = senderId, ReceiverId = receiver.Id, ChatRoomId = chatRoom.Id, Body = body };

        var message = await messageService.CreateAsync(created);
        var messageViewModel = mapper.Map<MessageViewModel>(message);
        try
        {
            await Clients.User(receiver.Id.ToString()).SendAsync("ReceiveMessage", messageViewModel);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    #region
    public override async Task OnConnectedAsync()
    {
        await SetOnlineInformationAsync(true);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await SetOnlineInformationAsync(false);
        await base.OnDisconnectedAsync(exception);
    }

    private async Task SetOnlineInformationAsync(bool isOnline)
    {
        var userId = GetRequiredUserId();

        var user = await userService.GetByIdAsync(userId);
        user.IsOnline = isOnline;

        await userService.UpdateAsync(user);
    }

    private long GetRequiredUserId()
    {
        var userId = Context.User!.Claims.FirstOrDefault(c => c.Type.Equals("UserId"))?.Value;

        return Convert.ToInt64(userId);
    }
    #endregion
}