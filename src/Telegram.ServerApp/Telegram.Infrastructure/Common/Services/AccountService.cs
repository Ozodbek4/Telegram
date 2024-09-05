using AutoMapper;
using Microsoft.Extensions.Options;
using Telegram.Application.Common.Helper;
using Telegram.Application.Common.Models.Dtos;
using Telegram.Application.Common.Services;
using Telegram.Application.Common.Settings;
using Telegram.Domain.Entities;

namespace Telegram.Infrastructure.Common.Services;

public class AccountService(IUserService userService, ITokenGeneratorService tokenGeneratorService, IPasswordHasher passwordHasher, IMapper mapper, IOptions<JwtSettings> jwtSettings) : IAccountService
{
    public JwtSettings JwtSettings { get; set; } = jwtSettings.Value;

    public async ValueTask<bool> SignUpAsync(SignUpDetails user, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        var u = mapper.Map<User>(user);
        var newUser = await userService.CreateAsync(u, saveChanges, cancellationToken);

        return newUser is not null;
    }

    public async ValueTask<string> SignInAsync(SignInDetails user, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        var foundUser = await userService.GetByUserNameAsync(user.UserName, saveChanges, cancellationToken);

        if (foundUser is null || !passwordHasher.Verify(user.Password, foundUser.Password))
            throw new Exception("Username or password is wrong");

        return await tokenGeneratorService.GenerateToken(foundUser);
    }
}