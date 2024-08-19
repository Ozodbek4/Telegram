using Telegram.Application.Common.Models.Dtos;

namespace Telegram.Application.Common.Services;

public interface IAccountService
{
    ValueTask<bool> SignUpAsync(SignUpDetails user, bool saveChanges = true, CancellationToken cancellationToken = default);

    ValueTask<string> SignInAsync(SignInDetails user, bool saveChanges = true, CancellationToken cancellationToken = default);
}