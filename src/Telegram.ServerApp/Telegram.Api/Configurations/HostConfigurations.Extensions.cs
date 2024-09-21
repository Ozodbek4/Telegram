using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using Telegram.Application.Common.Helper;
using Telegram.Application.Common.Services;
using Telegram.Application.Common.Settings;
using Telegram.Application.Services;
using Telegram.Infrastructure.Common.Caching.Brokers;
using Telegram.Infrastructure.Common.Helper;
using Telegram.Infrastructure.Common.Services;
using Telegram.Infrastructure.Common.Settings;
using Telegram.Infrastructure.Services;
using Telegram.Persistence.Caching.Brokers;
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
        builder.Services.AddCors(option =>
        {
            option.AddPolicy("AllowSpecificOrigin",
            builder => builder.WithOrigins("http://localhost:7029") // Adjust to your frontend URL
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials());
        });

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
            .UseAuthentication()
            .UseAuthorization()
            .UseRouting()
            .UseHsts()
            .UseCors("AllowSpecificOrigin")
            .UseHttpsRedirection();

        app
            .MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}");

        return app;
    }
    #endregion

    private static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddScoped<IUserService, UserService>()
            .AddScoped<IMessageService, MessageService>()
            .AddScoped<IChatService, ChatService>()
            .AddScoped<IChatOrchestrationService, ChatOrchestrationService>();

        return builder;
    }

    private static WebApplicationBuilder AddPersistence(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IMessageRepository, MessageRepository>()
            .AddScoped<IChatRepository, ChatRepository>();

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

    private static WebApplicationBuilder AddCaching(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<CacheSettings>(builder.Configuration.GetSection(nameof(CacheSettings)));

        builder.Services.AddSingleton<ICacheBroker, RedisDistributedCacheBroker>();

        builder.Services
            .AddStackExchangeRedisCache(option =>
            {
                option.Configuration = builder.Configuration.GetConnectionString("RedisConnectionString");
                option.InstanceName = "TelegramApp";
            });

        return builder;
    }

    private static WebApplicationBuilder AddIdentityInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddScoped<IAccountService, AccountService>()
            .AddScoped<ITokenGeneratorService, TokenGeneratorService>()
            .AddScoped<IPasswordHasher, PasswordHasher>();

        builder.Services
            .Configure<JwtSettings>(builder.Configuration.GetSection(nameof(JwtSettings)));

        var jwtSettings = builder.Configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>() ??
            throw new InvalidOperationException("JwtSettings is not configured.");

        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;

                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = jwtSettings.ValidateIssuer,
                    ValidIssuer = jwtSettings.ValidIssuer,
                    ValidAudience = jwtSettings.ValidAudience,
                    ValidateAudience = jwtSettings.ValidateAudience,
                    ValidateLifetime = jwtSettings.ValidateLifeTime,
                    ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                };
                o.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["token"];
                        return Task.CompletedTask;
                    }
                };

            });

        return builder;
    }
}