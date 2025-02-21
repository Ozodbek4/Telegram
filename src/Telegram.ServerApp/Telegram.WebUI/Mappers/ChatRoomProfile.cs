using AutoMapper;
using Telegram.Domain.Entities;
using Telegram.WebUI.Models.ChatRooms;
using Telegram.WebUI.Models.Messages;
using Telegram.WebUI.Models.Users;

namespace Telegram.WebUI.Mappers;

public class ChatRoomProfile : Profile
{
    public ChatRoomProfile()
    {
        CreateMap<CreateChatRoomModel, ChatRoom>();
        CreateMap<UpdateChatRomModel, ChatRoom>();
        CreateMap<ChatRoom, ChatRoomViewModel>()
            .ConstructUsing((src, context) => new ChatRoomViewModel
            {
                FirstUser = context.Mapper.Map<UserViewModel>(src.FirstUser),
                SecondUser = context.Mapper.Map<UserViewModel>(src.SecondUser),
                LastMessage = context.Mapper.Map<MessageViewModel>(src.LastMessage)
            })
            .ReverseMap();
    }
}