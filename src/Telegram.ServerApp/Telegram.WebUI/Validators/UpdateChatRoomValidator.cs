using FluentValidation;
using Telegram.WebUI.Models.ChatRooms;

namespace Telegram.WebUI.Validators;

public class UpdateChatRoomValidator : AbstractValidator<UpdateChatRomModel>
{
    public UpdateChatRoomValidator()
    {

    }
}