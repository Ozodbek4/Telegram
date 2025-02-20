using Telegram.Application.Common.Exceptions;
using Telegram.Application.Common.Identity;
using Telegram.Application.Services;
using Telegram.Domain.Entities;
using Telegram.Persistence.UnitOfWorks;

namespace Telegram.Infrastructure.Common.Identity;

public class AccountService(
    IUnitOfWork unitOfWork,
    IPasswordHasherService passwordHasherService,
    ITokenGeneratorService tokenGeneratorService) : IAccountService
{
    public async Task<(User User, string Token)> SignInAsync(string userName, string password, CancellationToken cancellationToken = default)
    {
        var exist = await unitOfWork.Users.SelectAsync(entity => entity.UserName == userName && !entity.IsDeleted)
            ?? throw new NotFoundException(nameof(User), userName);

        if (!await passwordHasherService.VerifyPassword(exist.Password, password))
            throw new CustomException("User name or password is wrong.");

        var token = await tokenGeneratorService.GenerateTokenAsync(exist, cancellationToken);

        return new(exist, token);
    }
}