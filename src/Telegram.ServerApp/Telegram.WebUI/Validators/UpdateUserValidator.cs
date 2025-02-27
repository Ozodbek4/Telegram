using FluentValidation;
using Telegram.WebUI.Models.Users;

namespace Telegram.WebUI.Validators;

public class UpdateUserValidator : AbstractValidator<UpdateUserModel>
{
    public UpdateUserValidator()
    {
        RuleFor(entity => entity.FirstName)
            .NotNull().NotEmpty().WithMessage("First name is required.")
            .MaximumLength(100).WithMessage("First name cannot exceed 100 characters.");

        RuleFor(entity => entity.LastName)
            .NotNull().NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters.");

        RuleFor(entity => entity.UserName)
            .NotNull().NotEmpty().WithMessage("User name is required.")
            .MinimumLength(6).WithMessage("User name must be at least 6 characters long.")
            .MaximumLength(100).WithMessage("User name cannot exceed 100 characters.");
    }
}