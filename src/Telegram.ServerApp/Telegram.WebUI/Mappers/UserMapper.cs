using AutoMapper;
using Telegram.Domain.Entities;
using Telegram.WebUI.Models.Users;

namespace Telegram.WebUI.Mappers;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<CreateUserModel, User>();
        CreateMap<UpdateUserModel, User>();
        CreateMap<UserViewModel, User>().ReverseMap();
    }
}