using Microsoft.Extensions.Options;
using Telegram.Application.Common.Models.Dtos;
using Telegram.Application.Common.Settings;

namespace Telegram.Application.Common.Services;

public interface IAccountService
{
    JwtSettings JwtSettings { get; set; }

    ValueTask<bool> SignUpAsync(SignUpDetails user, bool saveChanges = true, CancellationToken cancellationToken = default);

    ValueTask<string> SignInAsync(SignInDetails user, bool saveChanges = true, CancellationToken cancellationToken = default);
}