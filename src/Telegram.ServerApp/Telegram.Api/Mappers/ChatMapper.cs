using AutoMapper;
using Telegram.Api.Models.Dtos;
using Telegram.Domain.Entities;

namespace Telegram.Api.Mappers;

public class ChatMapper : Profile
{
    public ChatMapper()
    {
        CreateMap<Chat, ChatDto>().ReverseMap();
    }
}