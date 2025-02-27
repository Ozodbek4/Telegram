using FluentValidation;
using Telegram.WebUI.Models.Login;

namespace Telegram.WebUI.Validators;

public class SignInModelValidator : AbstractValidator<SignInModel>
{
    public SignInModelValidator()
    {
        RuleFor(entity => entity.UserName)
            .NotNull().NotEmpty().WithMessage("User name is required.")
            .MinimumLength(6).WithMessage("User name must be at least 6 characters long.")
            .MaximumLength(100).WithMessage("User name cannot exceed 100 characters.");

        RuleFor(entity => entity.Password)
            .NotNull().NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
            .MaximumLength(100).WithMessage("Password cannot exceed 100 characters.");
    }
}