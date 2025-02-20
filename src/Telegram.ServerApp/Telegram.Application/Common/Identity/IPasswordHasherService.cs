namespace Telegram.Application.Common.Identity;

public interface IPasswordHasherService
{
    Task<string> HashPassword(string password, CancellationToken cancellationToken = default);

    Task<bool> VerifyPassword(string hashedPassword, string providedPassword, CancellationToken cancellationToken = default);
}