using Telegram.Application.Common.Exceptions;
using Telegram.Application.Common.Identity;
using Telegram.Application.Services;
using Telegram.Domain.Entities;
using Telegram.Persistence.UnitOfWorks;

namespace Telegram.Infrastructure.Common.Identity;

public class AccountService(IUnitOfWork unitOfWork, ITokenGeneratorService tokenGeneratorService) : IAccountService
{
    public async Task<(User User, string Token)> SignInAsync(string userName, string password, CancellationToken cancellationToken = default)
    {
        var exist = await unitOfWork.Users.SelectAsync(entity => entity.UserName == userName && !entity.IsDeleted)
            ?? throw new NotFoundException(nameof(User), userName);

        var token = await tokenGeneratorService.GenerateTokenAsync(exist, cancellationToken);

        return new(exist, token);
    }
}