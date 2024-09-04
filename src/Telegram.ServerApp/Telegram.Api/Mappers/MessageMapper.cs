using AutoMapper;
using Telegram.Api.Models.Dtos;
using Telegram.Domain.Entities;

namespace Telegram.Api.Mappers;

public class MessageMapper : Profile
{
    public MessageMapper()
    {
        CreateMap<Message, MessageDto>().ReverseMap();
    }
}