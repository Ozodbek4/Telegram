using Telegram.Domain.Entities;

namespace Telegram.Application.Common.Identity;

public interface IAccountService
{
    public Task<(User User, string Token)> SignInAsync(string userName, string password, CancellationToken cancellationToken = default);
}