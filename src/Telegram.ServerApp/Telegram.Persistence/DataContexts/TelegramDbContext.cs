using Microsoft.EntityFrameworkCore;
using Telegram.Domain.Entities;

namespace Telegram.Persistence.DataContexts;

public class TelegramDbContext(DbContextOptions<TelegramDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TelegramDbContext).Assembly);
    }
}