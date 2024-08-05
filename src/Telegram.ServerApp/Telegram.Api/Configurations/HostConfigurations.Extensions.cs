using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Telegram.Api.Mappers;
using Telegram.Application.Common.Services;
using Telegram.Infrastructure.Common.Services;
using Telegram.Persistence.DataContexts;
using Telegram.Persistence.Interceptors;
using Telegram.Persistence.Repositories;
using Telegram.Persistence.Repositories.Interfaces;

namespace Telegram.Api.Configurations;

public static partial class HostConfigurations
{
    private static readonly ICollection<Assembly> Assemblies;

    static HostConfigurations()
    {
        Assemblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies().Select(Assembly.Load).ToList();
        Assemblies.Add(Assembly.GetExecutingAssembly());
    }

    #region
    private static WebApplicationBuilder AddDevTools(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen();

        return builder;
    }

    private static WebApplicationBuilder AddExposers(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddRouting(options => options.LowercaseUrls = true)
            .AddControllers();
            

        return builder;
    }

    private static WebApplication UseDevTools(this WebApplication app)
    {
        app
            .UseSwagger()
            .UseSwaggerUI();

        return app;
    }

    private static WebApplication UseExposers(this WebApplication app)
    {
        app
            .MapControllers();
        
        return app;
    }
    #endregion
    
    private static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddScoped<IUserService, UserService>();

        return builder;
    }

    private static WebApplicationBuilder AddPersistence(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddScoped<IUserRepository, UserRepository>();

        builder.Services
            .AddScoped<AuditableInterceptor>()
            .AddScoped<SoftDeletedInterceptor>();

        builder.Services
            .AddDbContext<TelegramDbContext>((provider, option) =>
            {
                option.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));

                option
                    .AddInterceptors(provider.CreateScope().ServiceProvider.GetRequiredService<AuditableInterceptor>())
                    .AddInterceptors(provider.CreateScope().ServiceProvider.GetRequiredService<SoftDeletedInterceptor>());
            });

        return builder;
    }

    private static WebApplicationBuilder AddMapper(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddAutoMapper(Assemblies);

        return builder;
    }
}