using Telegram.Domain.Entities;

namespace Telegram.Application.Common.Identity;

public interface ITokenGeneratorService
{
    Task<string> GenerateTokenAsync(User user, CancellationToken cancellationToken = default);
}