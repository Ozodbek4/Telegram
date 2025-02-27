using FluentValidation;
using Telegram.WebUI.Models.ChatRooms;

namespace Telegram.WebUI.Validators;

public class CreateChatRoomValidator : AbstractValidator<CreateChatRoomModel>
{
    public CreateChatRoomValidator()
    {
        RuleFor(entity => entity.FirstUserId)
            .GreaterThan(0)
            .WithMessage("ID must be greater than 0.");

        RuleFor(entity => entity.SecondUserId)
            .GreaterThan(0)
            .WithMessage("ID must be greater than 0.");
    }
}