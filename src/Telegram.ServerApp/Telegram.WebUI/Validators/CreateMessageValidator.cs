using FluentValidation;
using Telegram.WebUI.Models.Messages;

namespace Telegram.WebUI.Validators;

public class CreateMessageValidator : AbstractValidator<CreateMessageModel>
{
    public CreateMessageValidator()
    {
        RuleFor(entity => entity.SenderId)
            .GreaterThan(0)
            .WithMessage("ID must be greater than 0.");

        RuleFor(entity => entity.ReceiverId)
            .GreaterThan(0)
            .WithMessage("ID must be greater than 0.");

        RuleFor(entity => entity.Body)
            .NotEmpty().NotNull()
            .WithMessage("The message must not be null or empty.");
    }
}