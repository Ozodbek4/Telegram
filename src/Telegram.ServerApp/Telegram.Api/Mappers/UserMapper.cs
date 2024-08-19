using AutoMapper;
using Telegram.Api.Models.Dtos;
using Telegram.Application.Common.Models.Dtos;
using Telegram.Domain.Entities;

namespace Telegram.Api.Mappers;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<User, UserDto>().ReverseMap();

        CreateMap<User, SignUpDetails>().ReverseMap();
    }
}
