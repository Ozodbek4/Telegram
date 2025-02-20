using FluentValidation;
using FluentValidation.Results;

namespace Telegram.Application.Common.Extensions;

public static class ValidationExtensions
{
    public static async Task<ValidationResult> EnsureValidationAsync<TObject>(
        this IValidator<TObject> validator,
        TObject instance,
        CancellationToken cancellationToken = default)
    {
        var result = await validator.ValidateAsync(instance, cancellationToken);

        if (!result.IsValid)
            throw new Exceptions.ValidationException(result.Errors);

        return result;
    }
}