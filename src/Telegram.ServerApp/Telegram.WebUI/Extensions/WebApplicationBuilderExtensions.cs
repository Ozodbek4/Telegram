using Telegram.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Telegram.Persistence;
using Telegram.WebUI.ExceptionHandlers;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Telegram.WebUI.Middlewares;
using System.Reflection;

namespace Telegram.WebUI.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddWebApplicationBuilder(this WebApplicationBuilder builder)
    {
        builder.Services.AddPresentation();
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddPersistence(builder.Configuration);

        return builder;
    }

    #region
    private static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddDevTools();
        services.AddExposers();
        services.AddSwagger();
        services.AddMapper();

        return services;
    }

    private static IServiceCollection AddDevTools(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        return services;
    }

    private static IServiceCollection AddExposers(this IServiceCollection services)
    {
        services.AddProblemDetails();
        services.AddExceptionHandler<AlreadyExistsExceptionHandler>();
        services.AddExceptionHandler<ArgumentIsNotValidExceptionHandler>();
        services.AddExceptionHandler<CustomExceptionHandler>();
        services.AddExceptionHandler<NotFoundExceptionHandler>();
        services.AddExceptionHandler<ValidationExceptionHandler>();
        services.AddExceptionHandler<InternalServerExceptionHandler>();

        services.AddControllers(options =>
            options.Conventions.Add(new RouteTokenTransformerConvention(new RouteTransformer())));

        services.AddRouting(options =>
        {
            options.LowercaseUrls = true;
        });

        services.AddAuthentication();
        services.AddAuthorization();

        return services;
    }

    private static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                BearerFormat = "JWT",
                Name = "JWT Authentication",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Description = "Put **_ONLY_** your JWT Bearer token on the textbox below!",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme,
                }
            };

            options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { jwtSecurityScheme, Array.Empty<string>() },
            });
        });

        return services;
    }

    private static IServiceCollection AddMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        return services;
    }
    #endregion
}