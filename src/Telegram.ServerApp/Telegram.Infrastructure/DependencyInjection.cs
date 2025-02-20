using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Application.Common.Identity;
using Telegram.Application.Services;
using Telegram.Infrastructure.Common.Identity;
using Telegram.Infrastructure.Services;

namespace Telegram.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddIdentity(configuration)
            .AddNotification()
            .AddServices();

        return services;
    }

    private static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITokenGeneratorService, TokenGeneratorService>();
        services.AddScoped<IPasswordHasherService, PasswordHasherService>();
        services.AddScoped<IAccountService, AccountService>();


        return services;
    }

    private static IServiceCollection AddNotification(this IServiceCollection services)
    {
        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}