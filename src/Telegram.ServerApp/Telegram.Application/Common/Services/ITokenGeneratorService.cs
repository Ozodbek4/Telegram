using Telegram.Domain.Entities;

namespace Telegram.Application.Common.Services;

public interface ITokenGeneratorService
{
    ValueTask<string> GenerateToken(User user, CancellationToken cancellationToken = default);
}