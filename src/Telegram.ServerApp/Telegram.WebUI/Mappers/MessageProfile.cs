using AutoMapper;
using Telegram.Domain.Entities;
using Telegram.WebUI.Models.Messages;
using Telegram.WebUI.Models.Users;

namespace Telegram.WebUI.Mappers;

public class MessageProfile : Profile
{
    public MessageProfile()
    {
        CreateMap<CreateMessageModel, Message>();
        CreateMap<UpdateMessageModel, Message>();
        CreateMap<Message, MessageViewModel>()
            .ConstructUsing((src, context) => new MessageViewModel
            {
                Sender = context.Mapper.Map<UserViewModel>(src.Sender),
                Receiver = context.Mapper.Map<UserViewModel>(src.Receiver),
            });
    }
}