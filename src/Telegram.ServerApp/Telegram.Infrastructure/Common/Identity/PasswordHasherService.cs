using Telegram.Application.Common.Identity;
using BC = BCrypt.Net.BCrypt;

namespace Telegram.Infrastructure.Common.Identity;

public class PasswordHasherService : IPasswordHasherService
{
    private const int _workFactor = 8;

    public Task<string> HashPassword(string password, CancellationToken cancellationToken = default) =>
        Task.FromResult(BC.HashPassword(password, workFactor: _workFactor));

    public Task<bool> VerifyPassword(string hashedPassword, string providedPassword, CancellationToken cancellationToken = default) =>
        Task.FromResult(BC.Verify(providedPassword, hashedPassword));
}