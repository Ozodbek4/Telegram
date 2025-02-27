using FluentValidation;
using Telegram.WebUI.Models.Messages;

namespace Telegram.WebUI.Validators;

public class UpdateMessageValidator : AbstractValidator<UpdateMessageModel>
{
    public UpdateMessageValidator()
    {
        RuleFor(entity => entity.Id)
            .GreaterThan(0)
            .WithMessage("ID must be greater than 0.");

        RuleFor(entity => entity.Body)
            .NotEmpty().NotNull()
            .WithMessage("The message must not be null or empty.");
    }
}