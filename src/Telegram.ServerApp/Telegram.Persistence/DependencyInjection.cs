using Telegram.Domain.Entities;
using Telegram.Persistence.DataContexts;
using Telegram.Persistence.Interceptors;
using Telegram.Persistence.Repositories;
using Telegram.Persistence.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Telegram.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultDbConnection"));
            options.AddInterceptors(new AuditableInterceptor());
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IRepository<User>, Repository<User>>();

        return services;
    }
}